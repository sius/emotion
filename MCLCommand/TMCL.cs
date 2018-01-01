namespace TMCLDirect
{
    public static class TMCL
    {
        // Opcodes of all TMCL commands that can be used in direct mode

        // ***********************************
        // Motion commands
        // ***********************************
        /// <summary>
        /// Rotate right
        /// </summary>
        public const int ROR = 1;
        /// <summary>
        /// Rotate left
        /// </summary>
        public const int ROL = 2;
        /// <summary>
        /// Motor stop
        /// </summary>
        public const int MST = 3;
        /// <summary>
        /// Move to position
        /// </summary>
        public const int MVP = 4;
        /// <summary>
        /// Reference search
        /// </summary>
        public const int RFS = 13;
        /// <summary>
        /// Store coordinate
        /// </summary>
        public const int SCO = 30;
        /// <summary>
        /// Get coordinate
        /// </summary>
        public const int GCO = 31;
        /// <summary>
        /// Capture coordinate
        /// </summary>
        public const int CCO = 32;

        // ***********************************
        // Parameter commands
        // ***********************************
        /// <summary>
        /// Set axis parameter
        /// </summary>
        public const int SAP = 5;
        /// <summary>
        /// Get axis parameter
        /// </summary>
        public const int GAP = 6;
        /// <summary>
        /// Store axis parameter into EEPROM
        /// </summary>
        public const int STAP = 7;
        /// <summary>
        /// Restore axis parameter from EEPROM
        /// </summary>
        public const int RSAP = 8;
        /// <summary>
        /// Set global parameter
        /// </summary>
        public const int SGP = 9;
        /// <summary>
        /// Get global parameter
        /// </summary>
        public const int GGP = 10;
        /// <summary>
        /// Store global parameter into EEPROM
        /// </summary>
        public const int STGP = 11;
        /// <summary>
        /// Restore global parameter from EEPROM
        /// </summary>
        public const int RSGP = 12;

        // ***********************************
        // I/O port commands
        // ***********************************
        /// <summary>
        /// Set output
        /// </summary>
        public const int SIO = 14;
        /// <summary>
        /// Get input
        /// </summary>
        public const int GIO = 15;
        /// <summary>
        /// Access to external SPI device
        /// </summary>
        public const int SAC = 29;

        // ***********************************
        // Control functions
        // ***********************************
        public const int GET_FIRMWARE_VERSION = 136;

        // ***********************************
        // Command type/options
        // ***********************************

        public const int MVP_TYPE_ABS = 0;
        public const int MVP_TYPE_REL = 1;
        public const int MVP_TYPE_COORD = 2;

        //Opcodes of TMCL control functions (to be used to run or abort a TMCL program in the module)
        public const int APPL_STOP = 128;
        public const int APPL_RUN = 129;
        public const int APPL_RESET = 131;

        //Options for MVP commandds
        public const int MVP_ABS = 0;
        public const int MVP_REL = 1;
        public const int MVP_COORD = 2;

        //Options for RFS command
        public const int RFS_START = 0;
        public const int RFS_STOP = 1;
        public const int RFS_STATUS = 2;

        // Output ports
        public const int DOUT0 = 0;
        public const int DOUT1 = 1;
        public const int DOUT2 = 2;
        public const int DOUT3 = 3;
        public const int DOUT4 = 4;
        public const int DOUT5 = 5;
        public const int DOUT6 = 6;
        public const int DOUT7 = 7;

        // Input ports
        public const int ADIN0 = 0;
        public const int ADIN1 = 1;
        public const int ADIN2 = 2;
        public const int ADIN3 = 3;
        public const int ADIN4 = 4;
        public const int ADIN5 = 5;
        public const int ADIN6 = 6;
        public const int ADIN7 = 7;
        public const int OPTO1 = 8;
        public const int OPTO2 = 9;
        public const int SHUTDOWN = 10;

        // Bus number

        /// <summary>
        /// output direct value
        /// </summary>
        public const int SPI_SEL0 = 0;

        // <summary>
        /// output direct value
        /// </summary>
        public const int SPI_SEL1 = 2;
        // <summary>
        /// output direct value
        /// </summary>
        public const int SPI_SEL2 = 3;

        /// <summary>
        /// output contents of accumulator
        /// </summary>
        public const int SPI_SEL0_ACC = 128;

        /// <summary>
        /// output contents of accumulator
        /// </summary>
        public const int SPI_SEL1_ACC = 130;

        /// <summary>
        /// output contents of accumulator
        /// </summary>
        public const int SPI_SEL2_ACC = 131;


        // ***********************************
        // Status Codes
        // ***********************************

        /// <summary>
        /// successfully executed, no error
        /// </summary>
        public const int SC_OK = 100;

        /// <summary>
        /// Command loaded into TMCL program EEPROM
        /// </summary>
        public const int SC_LOADED = 100;
        public const int SC_WRONG_CHECKSUM = 1;
        public const int SC_INVALID_COMMAND = 2;
        public const int SC_WRONG_TYPE = 3;
        public const int SC_INVALID_VALUE = 4;

        /// <summary>
        /// Configuration EEPROM locked
        /// </summary>
        public const int SC_CONFIG_EEPROM_LOCKED = 5;

        /// <summary>
        /// Command not available
        /// </summary>
        public const int SC_CMD_NOT_AVAILABLE = 6;

        // ***********************************
        // Ranges/Limits
        // ***********************************
        public const int MAX_POS = 8388608;
        public const int MIN_POS = -8388608;
        public const int MAX_VELOCITY = 8191;
        public const int MIN_VELOCITY = 0;
        public const int MAX_CO_NUM = 20;
        public const int MIN_CO_NUM = 0;
        public const int MAX_ANALOGUE_MODE_INPUT = 1023;
        public const int MIN_ANALOGUE_MODE_INPUT = 0;

        public const int FALSE = 0;
        public const int TRUE = 1;

        public const int LOW = 0;
        public const int HIGH = 1;
    }
}
