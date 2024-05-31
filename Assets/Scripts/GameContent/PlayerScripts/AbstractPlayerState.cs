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
        
        public ControllerState StateFlag { get; }

        #endregion
        
        #region constructor

        protected AbstractPlayerState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go)
        {
            StateFlag = state;
            _currentDir = _isoForwardDir;
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
            
            _prevPos = _goRef.transform.position;
            _nextPos = _goRef.transform.position;
        }

        public override sbyte OnUpdate()
        {
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
        
        protected CharacterController _cc;

        protected BasePlayerDatasSO _datasSo;

        protected InterCheckerState _checker;
        
        protected Vector3 _currentDir;
        
        protected Vector3 _inputDir;

        protected readonly Vector3 _isoRightDir = new(1, 0, -1);
        
        protected readonly Vector3 _isoForwardDir = new(1, 0, 1);

        private Vector3 _prevPos;

        private Vector3 _nextPos;

        #endregion
    }
}