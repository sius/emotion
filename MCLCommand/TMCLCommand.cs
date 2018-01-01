using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace TMCLDirect
{
    public static class TMCLCommand
    {
        public static SerialPort CreateRS232Port(string portName, int baudRate = 9600)
        {
            return new SerialPort
            {
                PortName = portName,
                BaudRate = baudRate,
                Parity = Parity.None,
                StopBits = StopBits.One,
                WriteBufferSize = 8,
                DiscardNull = false,
                RtsEnable = true,
                ReadTimeout = int.MaxValue,
            };
        }

        /// <summary>
        /// Send a binary TMCL command
        /// </summary>
        /// <param name="port"></param>
        /// <param name="opcode">the TMCL opcode/command</param>
        /// <param name="address">address of the module (factory default is 1)</param>
        /// <param name="type">the "Type" parameter of the TMCL opcode/command (set to 0 if unused)</param>
        /// <param name="motor">the motor number (set to 0 if unused)</param>
        /// <param name="value">the "Value" parameter (depending on the opcode/command, set to 0 if unused)</param>
        public static void Send(this SerialPort port, byte opcode, byte address = 1, byte type = 0, byte motor = 0, int value = 0)
        {
            if (!port.IsOpen) port.Open();
            var buffer = CreateBinaryInstruction(address, opcode, type, motor, value);
            port.Write(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// Send a binary TMCL command and await async reply
        /// </summary>
        /// <param name="port"></param>
        /// <param name="opcode">the TMCL opcode/command</param>
        /// <param name="token">the CancellationToken token</param>
        /// <param name="address">address of the module (factory default is 1)</param>
        /// <param name="type">the "Type" parameter of the TMCL opcode/command (set to 0 if unused)</param>
        /// <param name="motor">the motor number (set to 0 if unused)</param>
        /// <param name="value">the "Value" parameter (depending on the opcode/command, set to 0 if unused)</param>
        public static async Task<TMCLReply> SendAsync(this SerialPort port, byte opcode, CancellationToken token, byte address = 1, byte type = 0, byte motor = 0, int value = 0)
        {
            if (!port.IsOpen) port.Open();
            var buffer = CreateBinaryInstruction(address, opcode, type, motor, value);
            Task<TMCLReply> reply = new Task<TMCLReply>(() =>
            {
                while (true)
                {
                    if (port.HasReply())
                    {
                        return port.GetReply();
                    }
                }
            }, token);
            reply.Start();
            port.Write(buffer, 0, buffer.Length);
            return await reply;
        }
        public static async Task<TMCLReply> SendAsync2(this TMCMotor motor, byte opcode, CancellationToken token, byte address = 1, byte type = 0, int value = 0)
        {
            if (!motor._port.IsOpen) motor._port.Open();
            var buffer = CreateBinaryInstruction(address, opcode, type, motor.Number, value);
            Task<TMCLReply> reply = new Task<TMCLReply>(() =>
            {
                while (true)
                {
                    if (motor.HasReply())
                    {
                        return motor.RaiseEvent();
                    }
                }
            }, token);
            reply.Start();
            motor._port.Write(buffer, 0, buffer.Length);
            return await reply;
        }
        public static bool HasReply(this SerialPort port)
        {
            return port.BytesToRead > 8;
        }
        public static TMCLReply GetReply(this SerialPort port)
        {
            var _ = new byte[9];
            port.Read(_, 0, 9);
            return new TMCLReply(_);
        }
        private static byte[] CreateBinaryInstruction(byte address, byte opcode, byte type, byte motor, int value)
        {
            byte[] _ = new byte[]
            {
                address, opcode, type, motor,
                (byte) (value >> 24),
                (byte) (value >> 16),
                (byte) (value >> 8),
                (byte) (value & 0xff),
                0
            };
            return CreateChecksum(_);
        }
        private static byte[] CreateChecksum(byte[] instruction)
        {
            for (int i = 0; i < 8; i++) instruction[8] += instruction[i];
            return instruction;
        }
    }
}
