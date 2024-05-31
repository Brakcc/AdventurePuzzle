using Cinemachine;
using GameContent.CameraScripts;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class CameraFocusState : AbstractPlayerState
    {
        #region constructor
        
        public CameraFocusState(GameObject go, ControllerState state, PlayerStateMachine pM) : base(go, state, pM)
        {
        }
        
        #endregion
        
        #region methodes
        
        public override void OnEnterState()
        {
            var c = new CameraDatas();
            Debug.Log($"{c.pivot}_{c.arm}");
        }

        public override void OnExitState()
        {
        }

        public override sbyte OnUpdate()
        {
            base.OnUpdate();
            
            if (_datasSo.cameraInput.action.IsPressed())
                return 0;

            stateMachine.SwitchState("idle");
            return 1;
        }

        #endregion

        #region fields

        private float _lerpCoef;

        #endregion
    }
}