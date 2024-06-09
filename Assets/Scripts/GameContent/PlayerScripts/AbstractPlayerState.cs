using System.Collections;
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

        protected Vector3 IsoRightDir => _playerMachine is null ? new Vector3(1, 0, -1) : _playerMachine.TransCamManager.transform.right;

        protected Vector3 IsoForwardDir => _playerMachine is null ? new Vector3(1, 0, 1) : _playerMachine.TransCamManager.transform.forward;
        
        public ControllerState StateFlag { get; }

        #endregion
        
        #region constructor

        protected AbstractPlayerState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go)
        {
            StateFlag = state;
            _currentDir = IsoForwardDir;
            _playerMachine = playerMachine;
            _cc = playerMachine.CharaCont;
            _checker = playerMachine.CheckerState;
            _datasSo = playerMachine.DatasSo;
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

            var p = _goRef.transform.position;
            _prevPos = p;
            _nextPos = p;
        }

        public override sbyte OnUpdate()
        {
            AnimationManager.SetLayerWeight(1, _playerMachine.MoveAnimLerpCoef);

            if (stateMachine == "move" && _playerMachine.MoveAnimLerpCoef < 1)
                _playerMachine.MoveAnimLerpCoef += Time.deltaTime;
            
            else if (stateMachine != "move" && _playerMachine.MoveAnimLerpCoef > 0)
                _playerMachine.MoveAnimLerpCoef -= Time.deltaTime;

            if (stateMachine == "camera")
            {
                if (_playerMachine.MoveAnimLerpCoef > 0)
                    _playerMachine.MoveAnimLerpCoef = 0;
                return 1;
            }

            if (_playerMachine.CamLerpCoef < 0.001f)
            {
                return 2;
            }
            
            _playerMachine.CamLerpCoef -= Time.deltaTime;
            
            _playerMachine.TransitionCamDatas.pivot.position = Vector3.Lerp(_playerMachine.InitCamDatas.pivot.position, 
                                                                            _playerMachine.CurrentCameraDatas.pivot.position,
                                                                            _playerMachine.CamLerpCoef);
            
            _playerMachine.TransitionCamDatas.arm.position = Vector3.Lerp(_playerMachine.InitCamDatas.arm.position, 
                                                                            _playerMachine.CurrentCameraDatas.arm.position,
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

        protected readonly PlayerStateMachine _playerMachine;
        
        protected Vector3 _currentDir;
        
        protected Vector3 _inputDir;

        private Vector3 _prevPos;

        private Vector3 _nextPos;

        #endregion
    }
}