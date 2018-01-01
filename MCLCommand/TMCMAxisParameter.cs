namespace TMCLDirect
{
    public enum TMCMAxisParameter
    {
        // Basic axis parameters
        TargetPosition = 0,
        ActualPosition = 1,
        TargetSpeed = 2,
        ActualSpeed = 3,
        MaxPositioningSpeed = 4,
        MaxAcceleration = 5,
        AbsMaxCurrent = 6,
        StandbyCurrent = 7,
        TargetPositionReached = 8,
        ReferenceSwitchStatus = 9,
        RightLimitSwitchStatus = 10,
        LeftLimitSwitchStatus = 11,
        RightLimitSwitchDisable = 12,
        LeftLimitSwitchDisable = 13,
        StepratePrescaler = 14,

        // Advanced axis parameters
        MinimumSpeed = 130,
        ActualAcceleration = 135,
        AccelerationThreshold = 136,
        AccelerationDivisor = 137,
        RampMode = 138,
        InterruptFlags = 139,
        MicrostepResolution = 140,
        RefSwitchTolerance = 141,
        SnapshotPosition = 142,
        MaxCurrentAtRest = 143,
        MaxCurrentAtLowAcceleration = 144,
        MaxCurrentAtHighAcceleration = 145,
        AccelerationFactor = 146,
        RefSwitchDisableFlag = 147,
        LimitSwitchDisableFlag = 148,
        SoftStopFlag = 149,
        PositionLatchFlag = 151,
        InterruptMask = 152,
        RampDivisor = 153,
        PulseDivisor = 154,
        ReferencingMode = 193,
        ReferencingSearchSpeed = 194,
        ReferencingSwitchSpeed = 195,
        Freewheeling = 204,
        StallDetectionThreshold = 205,
        ActualLoadValue = 206,
        DriverErrorFlags = 208
    }
}
