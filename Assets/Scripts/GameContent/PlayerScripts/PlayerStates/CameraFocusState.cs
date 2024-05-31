using Cinemachine;
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
            if (_playerMachine.CurrentCameraDatas.pivot == default)
            {
                stateMachine.SwitchState("idle");
                return;
            }

            _activeCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping = 
                new Vector3(Constants.VFXDatas.CameraDamping, 
                            Constants.VFXDatas.CameraDamping, 
                            Constants.VFXDatas.CameraDamping);
             
            _activeCamera.m_Follow = _playerMachine.CurrentCameraDatas.arm;
            _activeCamera.m_LookAt = _playerMachine.TransitionCamDatas.pivot;
        }

        public override void OnExitState()
        {
            _activeCamera.m_Follow = _playerMachine.InitCamDatas.arm;
            _activeCamera.m_LookAt = _playerMachine.TransitionCamDatas.pivot;
        }

        public override sbyte OnUpdate()
        {
            base.OnUpdate();

            SetCamPos();
            
            if (GetHeldInput() == 1)
                stateMachine.SwitchState("idle");

            return 0;
        }

        private void SetCamPos()
        {
            if (_playerMachine.CamLerpCoef < 0.99f)
                _playerMachine.CamLerpCoef += Time.deltaTime;

            _playerMachine.TransitionCamDatas.pivot.position = Vector3.Lerp(_playerMachine.InitCamDatas.pivot.position, 
                                                                            _playerMachine.CurrentCameraDatas.pivot.position,
                                                                            _playerMachine.CamLerpCoef);
        }
        
        private sbyte GetHeldInput()
        {
            return _datasSo.cameraInput.action.IsPressed() ? (sbyte)0 : (sbyte)1;
        }

        #endregion

        #region fields

        private Transform movingCamHolder;

        private Transform movingCamPivot;

        #endregion
    }
}