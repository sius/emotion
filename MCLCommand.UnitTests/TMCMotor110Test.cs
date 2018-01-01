using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Ports;
using System.Threading;
using System.Reflection;

namespace TMCLDirect
{
    [TestClass]
    public class TMCMotor110Test
    {
        const string PortName = "COM3"; 
        [TestMethod]
        public void Find()
        {
            foreach (var s in SerialPort.GetPortNames())
            {
                Console.WriteLine(s);
            }
        }
        [TestMethod]
        public void Direct()
        {
            using (var motor = new TMCMotor110(PortName))
            {
                var reply = motor.MoveAbsolute(0);
                Console.WriteLine(reply);
                reply = motor.RotateRight(1000);
                Console.WriteLine(reply);
                reply = motor.Stop();
                Console.WriteLine(reply);
                reply = motor.RotateLeft(1000);
                Console.WriteLine(reply);
                reply = motor.Stop();
                Console.WriteLine(reply);
                reply = motor.MoveAbsolute(0);
                Console.WriteLine(reply);
            }
        }
        [TestMethod]
        public void StartRFS()
        {
            using (var motor = new TMCMotor110(PortName))
            {
                var reply = motor.StartReferenceSearch();
                Thread.Sleep(1000);
                reply = motor.GetReferenceSearchStatus();
                Console.WriteLine(reply);
            }
        }
        [TestMethod]
        public void AbortRFS()
        {
            using (var motor = new TMCMotor110(PortName))
            {
                var reply = motor.AbortReferenceSearch();
                Console.WriteLine(reply);
            }
        }
        [TestMethod]
        public void RotateRight()
        {
            using (var motor = new TMCMotor110(PortName))
            {
                var reply = motor.RotateRight(2047);
                Console.WriteLine(reply);
                try
                { 
                    for (var i = 0; i < 5; i++)
                    {
                        foreach (PropertyInfo prop in motor.GetType().GetProperties())
                        {
                            if (prop.CanRead)
                            {
                                Console.WriteLine("{0}: {1}", prop.Name, prop.GetValue(motor));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    motor.Stop();
                }
                
            }
        }
        [TestMethod]
        public void RotateLeft()
        {
            using (var motor = new TMCMotor110(PortName))
            {
                var reply = motor.RotateLeft(2047);
                Console.WriteLine(reply);
            }
        }
        [TestMethod]
        public void MotorStop()
        {
            using (var motor = new TMCMotor110(PortName))
            {
                var reply = motor.Stop();
                Console.WriteLine(reply);
            }
        }

        private void Motor_MotorStopped(object sender, EventArgs e)
        {
            Console.WriteLine("Motor Stopped");
        }

        [TestMethod]
        public void MoveAbsolute()
        {
            using (var motor = new TMCMotor110(PortName))
            {
                var reply = motor.MoveAbsolute(0);
                Console.WriteLine(reply);
            }
                
        }
        [TestMethod]
        public void MoveRelative()
        {
            using (var motor = new TMCMotor110(PortName))
            {
                var reply = motor.MoveRelative(-207);
                Console.WriteLine(reply);
            }
        }
        [TestMethod]
        public void GetFirmwareReply()
        {
            using (var motor = new TMCMotor110(PortName))
            {
                var reply = motor.GetFirmwareVersion();
                Console.WriteLine(reply);
            }
        }
    }
}
