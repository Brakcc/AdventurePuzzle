using System;

namespace GameContent.PlayerScripts.PlayerStates
{
    [Flags]
    public enum ControllerState
    {
        idle = 0,
        move = 1,
        jump = 2,
        interact = 3,
        cancel = 4,
        fall = 5,
        locked = 6,
        lever = 7,
        camera = 8,
        cineIdle = 9,
        cineMove = 10
    }
}