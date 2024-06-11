using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates.ForcedStates
{
    public sealed class CinematicIdleForcedState : AbstractPlayerState
    {
        public CinematicIdleForcedState(GameObject go, ControllerState state, PlayerStateMachine pM) : base(go, state, pM)
        {
        }

        public override void OnEnterState()
        {
            AnimationManager.SetAnims("isWalking", false);
        }

        public override void OnExitState()
        {
            
        }
    }
}