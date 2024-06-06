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

            _playerMachine.InitCamManager.IsFocused = true;
            _playerMachine.TransCamManager.IsFocused = true;
        }

        public override void OnExitState()
        {
            _playerMachine.InitCamManager.IsFocused = false;
            _playerMachine.TransCamManager.IsFocused = false;
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
            
            _playerMachine.TransitionCamDatas.arm.position = Vector3.Lerp(_playerMachine.InitCamDatas.arm.position, 
                                                                            _playerMachine.CurrentCameraDatas.arm.position,
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