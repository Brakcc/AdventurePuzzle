using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class CameraFocusState : AbstractPlayerState
    {
        public CameraFocusState(GameObject go, ControllerState state) : base(go, state)
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