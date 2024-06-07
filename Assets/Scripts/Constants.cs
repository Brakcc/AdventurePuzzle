//NE PAS TOUCHER sauf si vous etes sur de ce que vous faites

public static class Constants
{
    internal const float GroundCheckSupLength = 0.1f;
    internal const float SpeedMultiplier = 10f;
    internal const float FallSpeedMultiplier = 0.5f;
    internal const float MinMoveInputValue = 0.1f;
    internal const float MinBlockMoveInputThreshold = 0.01f;
    internal const float ReloadLeverManipThreshold = 0.005f;
    internal const float MinLeverInputThreshold = 0.75f;
    internal const float MinPlayerRotationAngle = 0.05f;
    internal const float SecuValuUnderZero = -0.1f;
    internal const float PlayerHeight = 2;
    internal const float JumpTimerAfterInput = 0.15f;
    internal const float RecepMoveSpeedMultiplier = 4f;
    internal const float RecepBlockedSpeedThreshold = 0.1f;
    internal const float GrabGapThreshold = 0.25f;
    internal const float MinWorldMoveSpeedThreshold = 0.005f;
    internal const sbyte OrientationNumber = 4;
    internal const int BitFlagRBConstraintRotaPlan = 122;
    internal const int MaxWaveEmitterEnergyContaints = 3;
    internal const float MonoWaveBeforeDelay = 0.85f;
    internal const float MaxFallCounterWhileGrabThreshold = 0.5f;
    internal const float MaxBlockFallCounterThreshold = 0.005f;
    internal const float WaveEnergyDuration = 12.5f;
    internal const float FirstQuarterAngleValue = 45;
    internal const float ThirdQuarterAngleValue = 135;
    internal const float PiByFourRadVal = 0.7071067f;
    internal const float ReMapNavMeshDelay = 1;
    internal const float ActionBlockerThreshold = 2f;
    internal const int PlayerLayer = 31;
    internal static readonly sbyte[] XShapeNodeArray = { 1, 1, 1, 1 };
    public static class BoxCastBounds
    {
        internal const float SideBoxPosDeport = 0.05f;
        internal const float DownBoxPosDeport = 0.15f;
        internal const float SideBoxHalfExtent = 0.075f;
        internal const float SideBoxLengthCut = 0.1f;
        internal const float DownBoxHalfExtent = 0.15f;
        internal const float SideCastDist = 0.1f;
        internal const float DownCastDist = 0.15f;
    }

    public static class VFXDatas
    {
        internal const float BatteryPartLifeSpan = 1.85f;
        internal const float BetteryAppearLifeSpan = 6.5f;
        internal const float BatteryDisapLifeSpan = 4.5f;
        internal const float CameraDamping = 3.5f;
    }
}