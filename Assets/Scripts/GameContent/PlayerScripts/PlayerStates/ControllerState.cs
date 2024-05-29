using System;

namespace GameContent.PlayerScripts.PlayerStates
{
    [Flags]
    public enum ControllerState
    {
        idle = 0,
        move = 1,
        jump = 2,
        interact = 4,
        cancel = 8,
        fall = 16,
        locked = 32,
        lever = 64,
        camera = 128,
        cineIdle = 256,
        cineMove = 512
    }
}