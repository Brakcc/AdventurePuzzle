using System.Collections;
using Cinemachine;
using GameContent.CameraScripts;
using GameContent.PlayerScripts.PlayerDatas;
using GameContent.PlayerScripts.PlayerStates;
using GameContent.StateMachines;
using UnityEngine;

namespace GameContent.PlayerScripts
{
    public abstract class AbstractPlayerState : AbstractGenericState
    {
        #region properties
        
        protected bool IsGrounded => _cc.isGrounded;

        protected float Velocity => (_nextPos - _prevPos).magnitude;
        
        public ControllerState StateFlag { get; }

        #endregion
        
        #region constructor

        protected AbstractPlayerState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go)
        {
            StateFlag = state;
            _currentDir = _isoForwardDir;
            _playerMachine = playerMachine;
            _cc = playerMachine.CharaCont;
            _checker = playerMachine.CheckerState;
            _datasSo = playerMachine.DatasSo;
            _activeCamera = playerMachine.ActiveCamera;
        }

        ~AbstractPlayerState()
        {
            //Debug.Log($"{this} removed");
        }
        
        #endregion
        
        #region base methodes
        
        protected static float ClampSymmetric(float val, float clamper) => Mathf.Clamp(val, -clamper, clamper);

        #endregion
        
        #region methodes to herit

        public override void OnInit(GenericStateMachine machine)
        {
            stateMachine = machine;
            
            _prevPos = _goRef.transform.position;
            _nextPos = _goRef.transform.position;
        }

        public override sbyte OnUpdate()
        {
            if (stateMachine == "camera")
                return 1;

            if (_playerMachine.CamLerpCoef < 0.001f)
            {
                return 2;
            }

            _activeCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().Damping =
                new Vector3(_playerMachine.CamLerpCoef * Constants.VFXDatas.CameraDamping,
                            _playerMachine.CamLerpCoef * Constants.VFXDatas.CameraDamping,
                            _playerMachine.CamLerpCoef * Constants.VFXDatas.CameraDamping);
            _playerMachine.CamLerpCoef -= Time.deltaTime;
            
            _playerMachine.TransitionCamDatas.pivot.position = Vector3.Lerp(_playerMachine.InitCamDatas.pivot.position, 
                                                                            _playerMachine.CurrentCameraDatas.pivot.position,
                                                                            _playerMachine.CamLerpCoef);
            
            return 0;
        }
        
        public override sbyte OnFixedUpdate()
        {
            _prevPos = _nextPos;
            _nextPos = _goRef.transform.position;

            return 0;
        }

        public override IEnumerator OnCoroutine()
        {
            yield return null;
        }

        #endregion

        #region fields
        
        protected readonly CharacterController _cc;

        protected readonly BasePlayerDatasSO _datasSo;

        protected readonly InterCheckerState _checker;

        protected readonly CinemachineVirtualCamera _activeCamera;

        protected readonly PlayerStateMachine _playerMachine;

        protected CameraDatas _initCam;
        
        protected Vector3 _currentDir;
        
        protected Vector3 _inputDir;

        protected readonly Vector3 _isoRightDir = new(1, 0, -1);
        
        protected readonly Vector3 _isoForwardDir = new(1, 0, 1);

        private Vector3 _prevPos;

        private Vector3 _nextPos;

        #endregion
    }
}