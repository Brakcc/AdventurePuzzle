using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates.ForcedStates
{
    public class CinematicMoveForcedState : AbstractPlayerState
    {
        public CinematicMoveForcedState(GameObject go, ControllerState state) : base(go, state)
        {
        }

        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }
    }
}