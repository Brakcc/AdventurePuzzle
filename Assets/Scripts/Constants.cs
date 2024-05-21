//NE PAS TOUCHER sauf si vous etes sur de ce que vous faites

using System;

public static class Constants
{
    public const float GroundCheckSupLength = 0.1f;
    public const float SpeedMultiplier = 10f;
    public const float FallSpeedMultiplier = 0.5f;
    public const float MinMoveInputValue = 0.1f;
    public const float MinBlockMoveInputThreshold = 0.01f;
    public const float ReloadLeverManipThreshold = 0.005f;
    public const float MinLeverInputThreshold = 0.75f;
    public const float MinPlayerRotationAngle = 0.05f;
    public const float SecuValuUnderZero = -0.1f;
    public const float PlayerHeight = 2;
    public const float JumpTimerAfterInput = 0.15f;
    public const float RecepMoveSpeedMultiplier = 4f;
    public const float RecepBlockedSpeedThreshold = 0.1f;
    public const float GrabGapThreshold = 0.25f;
    public const float MinWorldMoveSpeedThreshold = 0.005f;
    public const short OrientationNumber = 4;
    public const int BitFlagRBConstraintRotaPlan = 122;
    public const float MaxFallCounterWhileGrabThreshold = 0.5f;
    public const float WaveEnergyDuration = 12.5f;
    public const float FirstQuarterAngleValue = 45;
    public const float ThirdQuarterAngleValue = 135;
    public const float PiByFourRadVal = 0.7071067f;
    public static readonly sbyte[] XShapeNodeArray = { 1, 1, 1, 1 };
}