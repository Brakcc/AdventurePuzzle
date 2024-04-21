using GameContent.Interactives.ClemInterTemplates;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class LockedOnInterState : AbstractPlayerState
    {
        #region constructor
        
        public LockedOnInterState(GameObject go) : base(go)
        {
        }
        
        #endregion

        #region methodes

        public override void OnInit()
        {
            
        }

        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _interRef = _checker.InterRef as ReceptorInter;
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            _stateMachine = null;
            _interRef = null;
        }

        public override void OnUpdate()
        {
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y).normalized;
            
            GetHoldInput();
        }

        public override void OnFixedUpdate()
        {
            OnMove();
        }

        #region holding methodes

        private void GetHoldInput()
        {
            if (_datasSo.interactInput.action.IsPressed())
                return;
            
            _stateMachine.OnSwitchState("move");
        }

        #endregion
        
        #region move methodes

        private void OnMove()
        {
            _currentDir = (_isoRightDir * _inputDir.x + _isoForwardDir * _inputDir.z).normalized;
            _cc.SimpleMove(_currentDir.normalized * (_datasSo.moveDatasSo.moveSpeed * Constants.SpeedMultiplier * Time.deltaTime));
        }
        
        #endregion

        #endregion

        #region fields

        private ReceptorInter _interRef;

        private LockDirectionMode _directionMode;

        #endregion

        private enum LockDirectionMode
        {
            TLToBR,
            BLToTR,
            None
        }
    }
}