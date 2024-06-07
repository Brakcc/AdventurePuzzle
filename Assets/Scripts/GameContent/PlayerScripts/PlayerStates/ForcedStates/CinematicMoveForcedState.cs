using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates.ForcedStates
{
    public sealed class CinematicMoveForcedState : AbstractPlayerState
    {
        public CinematicMoveForcedState(GameObject go, ControllerState state, PlayerStateMachine pM) : base(go, state, pM)
        {
        }
        
        public override void OnEnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExitState()
        {
            throw new System.NotImplementedException();
        }
    }
}