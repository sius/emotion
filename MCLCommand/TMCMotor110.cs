using System;
using System.IO.Ports;
using System.Threading;

namespace TMCLDirect
{
    public sealed class TMCMotor110 : TMCMotor
    {
        #region Ctor

        public TMCMotor110(string portName, byte number = 0) : base(portName, number)
        { }
        public TMCMotor110(SerialPort port, byte number = 0) : base(port, number)
        { }

        #endregion

        #region Advanced Axis parameter

        public int MinimumSpeed
        {
            get { return GetAxisParameter(TMCMAxisParameter.MinimumSpeed).Value; }
            set { SetAxisParameter(TMCMAxisParameter.MinimumSpeed, value); }
        }
        public int ActualAcceleration
        {
            get { return GetAxisParameter(TMCMAxisParameter.ActualAcceleration).Value; }
        }
        public int AccelerationThreshold
        {
            get { return GetAxisParameter(TMCMAxisParameter.AccelerationThreshold).Value; }
            set { SetAxisParameter(TMCMAxisParameter.AccelerationThreshold, value); }
        }
        public int AccelerationDivisor
        {
            get { return GetAxisParameter(TMCMAxisParameter.AccelerationDivisor).Value; }
            set { SetAxisParameter(TMCMAxisParameter.AccelerationDivisor, value); }
        }
        public int RampMode
        {
            get { return GetAxisParameter(TMCMAxisParameter.RampMode).Value; }
            set { SetAxisParameter(TMCMAxisParameter.RampMode, value); }
        }
        public int InterruptFlage
        {
            get { return GetAxisParameter(TMCMAxisParameter.InterruptFlags).Value; }
            set { SetAxisParameter(TMCMAxisParameter.InterruptFlags, value); }
        }
        public int MicrostepResolution
        {
            get { return GetAxisParameter(TMCMAxisParameter.MicrostepResolution).Value; }
            set { SetAxisParameter(TMCMAxisParameter.MicrostepResolution, value); }
        }
        // TODO

        #endregion
    }
}
