using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace TMCLDirect
{
    public abstract class TMCMotor : IDisposable
    {
        #region Event

        public delegate void TMCLReplyEventHandler(object sender, TMCLReplyEventArgs a);
        public event TMCLReplyEventHandler TMCLReplied;

        #endregion

        #region Fields

        internal SerialPort _port;
        protected CancellationTokenSource _tokenSource;

        #endregion

        #region Ctor

        public TMCMotor(string portName, byte number) : this(TMCLCommand.CreateRS232Port(portName), number)
        {
        }
        public TMCMotor(SerialPort port, byte number, long pollingIntervalMs = 50)
        {
            _port = port;
            Number = number;
        }

        internal bool HasReply()
        {
            return _port.HasReply();      
        }
        internal TMCLReply RaiseEvent()
        {
            var reply = _port.GetReply();
            if (TMCLReplied != null)
            {
                TMCLReplied(this, new TMCLReplyEventArgs(reply));
            }
            return reply;
        }
        #endregion

        #region Parameter commands

        /// <summary>
        /// Set axis parameter
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TMCLReply SetAxisParameter(TMCMAxisParameter parameterNumber, int value)
        {
            return SetAxisParameter((byte)parameterNumber, value);
        }
        /// <summary>
        /// Get axis parameter
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <returns></returns>
        public TMCLReply GetAxisParameter(TMCMAxisParameter parameterNumber)
        {
            return GetAxisParameter((byte)parameterNumber);
        }
        /// <summary>
        /// Store axis parameter into EEPROM
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <returns></returns>
        public TMCLReply StoreAxisParameter(TMCMAxisParameter parameterNumber)
        {
            return StoreAxisParameter((byte)parameterNumber);
        }
        /// <summary>
        /// Restore axis parameter from EEPROM
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <returns></returns>
        public TMCLReply RestoreAxisParameter(TMCMAxisParameter parameterNumber)
        {
            return RestoreAxisParameter((byte)parameterNumber);
        }

        #endregion

        #region Properties

        public byte Number { get; private set; }

        #endregion

        #region Axis parameter

        public int TargetPosition
        {
            get { return GetAxisParameter(TMCMAxisParameter.TargetPosition).Value; }
            set { SetAxisParameter(TMCMAxisParameter.TargetPosition, value); }
        }
        public int ActualPosition
        {
            get { return GetAxisParameter(TMCMAxisParameter.ActualPosition).Value; }
            set { SetAxisParameter(TMCMAxisParameter.ActualPosition, value); }
        }
        public int TargetSpeed
        {
            get { return GetAxisParameter(TMCMAxisParameter.TargetSpeed).Value; }
            set { SetAxisParameter(TMCMAxisParameter.TargetSpeed, value); }
        }
        public int ActualSpeedy
        {
            get { return GetAxisParameter(TMCMAxisParameter.ActualSpeed).Value; }
        }
        public int MaxPositioningSpeed
        {
            get { return GetAxisParameter(TMCMAxisParameter.MaxPositioningSpeed).Value; }
            set { SetAxisParameter(TMCMAxisParameter.MaxPositioningSpeed, value); }
        }
        public int MaxAcceleration
        {
            get { return GetAxisParameter(TMCMAxisParameter.MaxAcceleration).Value; }
            set { SetAxisParameter(TMCMAxisParameter.MaxAcceleration, value); }
        }
        public byte AbsMaxCurrent
        {
            get { return (byte)GetAxisParameter(TMCMAxisParameter.AbsMaxCurrent).Value; }
            set { SetAxisParameter(TMCMAxisParameter.AbsMaxCurrent, value); }
        }
        public byte StandbyCurrent
        {
            get { return (byte)GetAxisParameter(TMCMAxisParameter.StandbyCurrent).Value; }
            set { SetAxisParameter(TMCMAxisParameter.StandbyCurrent, value); }
        }
        public bool TargetPositionReached
        {
            get { return GetAxisParameter(TMCMAxisParameter.TargetPositionReached).Value == 1; }
        }
        public bool ReferenceSwitchStatus
        {
            get { return GetAxisParameter(TMCMAxisParameter.ReferenceSwitchStatus).Value == 1; }
        }
        public bool RightLimitSwitchStatus
        {
            get { return GetAxisParameter(TMCMAxisParameter.RightLimitSwitchStatus).Value == 1; }
        }
        public bool LeftLimitSwitchStatus
        {
            get { return GetAxisParameter(TMCMAxisParameter.LeftLimitSwitchStatus).Value == 1; }
        }
        public bool RightLimitSwitchDisable
        {
            get { return GetAxisParameter(TMCMAxisParameter.RightLimitSwitchDisable).Value == 1; }
            set { SetAxisParameter(TMCMAxisParameter.RightLimitSwitchDisable, value ? 1 : 0); }
        }
        public bool LeftLimitSwitchDisable
        {
            get { return GetAxisParameter(TMCMAxisParameter.LeftLimitSwitchDisable).Value == 1; }
            set { SetAxisParameter(TMCMAxisParameter.LeftLimitSwitchDisable, value ? 1 : 0); }
        }
        public int StepratePrescaler
        {
            get { return GetAxisParameter(TMCMAxisParameter.StepratePrescaler).Value; }
            set { SetAxisParameter(TMCMAxisParameter.StepratePrescaler, value); }
        }

        #endregion

        #region Motion Commands

        /// <summary>
        /// Rotate right
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public TMCLReply RotateRight(int velocity) 
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                ThrowIfNotInRange("velocity", TMCL.MIN_VELOCITY, TMCL.MAX_VELOCITY, velocity);
                return _port.SendAsync(TMCL.ROR, _tokenSource.Token, motor: Number, value: velocity).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            } 
            finally
            {
                _tokenSource.Cancel();
            }
            
            
        }
        /// <summary>
        /// Rotate left
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public TMCLReply RotateLeft(int velocity)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                ThrowIfNotInRange("velocity", TMCL.MIN_VELOCITY, TMCL.MAX_VELOCITY, velocity);
                return _port.SendAsync(TMCL.ROL, _tokenSource.Token, motor: Number, value: velocity).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Motor stop
        /// </summary>
        /// <returns></returns>
        public TMCLReply Stop()
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.MST, _tokenSource.Token, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Move to absolute poition
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public TMCLReply MoveAbsolute(int position)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                ThrowIfNotInRange("position", TMCL.MIN_POS, TMCL.MAX_POS, position);
                return _port.SendAsync(TMCL.MVP, _tokenSource.Token, type: TMCL.MVP_TYPE_ABS, motor: Number, value: position).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Move to relative position
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public TMCLReply MoveRelative(int offset)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                ThrowIfNotInRange("offset", TMCL.MIN_POS, TMCL.MAX_POS, offset);
                return _port.SendAsync(TMCL.MVP, _tokenSource.Token, type: TMCL.MVP_TYPE_REL, motor: Number, value: offset).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Move to coordinate position
        /// </summary>
        /// <param name="coordinateNumber"></param>
        /// <returns></returns>
        public TMCLReply MoveToCoordinate(byte coordinateNumber)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                ThrowIfNotInRange("coordinateNumber", TMCL.MIN_CO_NUM, TMCL.MAX_CO_NUM, coordinateNumber);
                return _port.SendAsync(TMCL.MVP, _tokenSource.Token, type: TMCL.MVP_TYPE_COORD, motor: Number, value: coordinateNumber).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        public TMCLReply StartReferenceSearch()
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.RFS, _tokenSource.Token, type: TMCL.RFS_START, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        public TMCLReply AbortReferenceSearch()
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.RFS, _tokenSource.Token, type: TMCL.RFS_STOP, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        public TMCLReply GetReferenceSearchStatus()
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.RFS, _tokenSource.Token, type: TMCL.RFS_STATUS, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Get coordinate
        /// </summary>
        /// <param name="coordinateNumber"></param>
        /// <returns></returns>
        public TMCLReply GetCoordinate(byte coordinateNumber)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                ThrowIfNotInRange("coordinateNumber", TMCL.MIN_CO_NUM, TMCL.MAX_CO_NUM, coordinateNumber);
                return _port.SendAsync(TMCL.GCO, _tokenSource.Token, type: coordinateNumber, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Set coordinate
        /// </summary>
        /// <param name="coordinateNumber"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public TMCLReply SetCoordinate(byte coordinateNumber, int position)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                ThrowIfNotInRange("coordinateNumber", TMCL.MIN_CO_NUM, TMCL.MAX_CO_NUM, coordinateNumber);
                ThrowIfNotInRange("position", TMCL.MIN_POS, TMCL.MAX_POS, position);
                return _port.SendAsync(TMCL.SCO, _tokenSource.Token, type: coordinateNumber, motor: Number, value: position).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Capture coordinate
        /// </summary>
        /// <param name="coordinateNumber"></param>
        /// <returns></returns>
        public TMCLReply CapturetCoordinate(byte coordinateNumber)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                ThrowIfNotInRange("coordinateNumber", TMCL.MIN_CO_NUM, TMCL.MAX_CO_NUM, coordinateNumber);
                return _port.SendAsync(TMCL.CCO, _tokenSource.Token, type: coordinateNumber, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }

        #endregion

        #region I/O port commands

        public TMCLReply SetOutput(TMCLOutputPort portNumber, TMCLFlag value)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.SIO, _tokenSource.Token, type: (byte)portNumber, motor: Number, value: (int)value).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }

        public TMCLReply GetInputOutput(TMCLInputPort portNumber)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.GIO, _tokenSource.Token, type: (byte)portNumber, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }

        public TMCLReply SPIBusAccess(TMCLBus busNumber, int sendData)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.GIO, _tokenSource.Token, type: (byte)busNumber, motor: Number, value: sendData).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }

        #endregion

        #region Parameter commands

        /// <summary>
        /// Set axis parameter
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TMCLReply SetAxisParameter(byte parameterNumber, int value)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.SAP, _tokenSource.Token, type: parameterNumber, motor: Number, value: value).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Get axis parameter
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <returns></returns>
        public TMCLReply GetAxisParameter(byte parameterNumber)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.GAP, _tokenSource.Token, type: parameterNumber, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Store axis parameter into EEPROM
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <returns></returns>
        public TMCLReply StoreAxisParameter(byte parameterNumber)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.STAP, _tokenSource.Token, type: parameterNumber, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Restore axis parameter from EEPROM
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <returns></returns>
        public TMCLReply RestoreAxisParameter(byte parameterNumber)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.RSAP, _tokenSource.Token, type: parameterNumber, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Set global parameter
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TMCLReply SetGlobalParameter(byte parameterNumber, int value)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.SGP, _tokenSource.Token, type: parameterNumber, motor: Number, value: value).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Get global parameter
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <returns></returns>
        public TMCLReply GetGlobalParameter(byte parameterNumber)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.GGP, _tokenSource.Token, type: parameterNumber, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Store global parameter into EEPROM
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <returns></returns>
        public TMCLReply StoreGlobalParameter(byte parameterNumber)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.STGP, _tokenSource.Token, type: parameterNumber, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        /// <summary>
        /// Restore global parameter from EEPROM
        /// </summary>
        /// <param name="parameterNumber"></param>
        /// <returns></returns>
        public TMCLReply RestoreGlobalParameter(byte parameterNumber)
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return _port.SendAsync(TMCL.RSGP, _tokenSource.Token, type: parameterNumber, motor: Number).Result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        #endregion

        #region Control functions
        public string GetFirmwareVersion()
        {
            _tokenSource = new CancellationTokenSource();
            try
            {
                return Encoding.ASCII.GetString(_port.SendAsync(TMCL.GET_FIRMWARE_VERSION, _tokenSource.Token, type: TMCL.LOW, motor: Number).Result.Buffer, 1, 8);
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            finally
            {
                _tokenSource.Cancel();
            }
        }
        #endregion

        #region Private methods

        private static void ThrowIfNotInRange(string name, int min, int max, int value)
        {
            if (value < min || value > max)
            {
                throw new ArgumentOutOfRangeException(name, string.Format("Value for {0} must be between {1} and {2}.", name, min, max));
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            try
            {
                //_timer.Dispose();
                if (_tokenSource != null)
                {
                    _tokenSource.Token.WaitHandle.WaitOne();
                }
                _port.Close();
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        #endregion
    }
}