using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class CameraFocusState : AbstractPlayerState
    {
        public CameraFocusState(GameObject go, ControllerState state) : base(go, state)
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