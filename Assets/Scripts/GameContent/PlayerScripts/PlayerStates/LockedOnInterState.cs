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

            _directionMode = GetBaseMoveDir();
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

        private LockDirectionMode GetBaseMoveDir()
        {
            var tempAngle = Vector3.Angle(Vector3.right, _goRef.transform.position - _interRef.pivot.position);

            return tempAngle switch
            {
                <= 45 or >= 135 => LockDirectionMode.BLToTR,
                < 135 or > 45 => LockDirectionMode.TLToBR,
                _ => LockDirectionMode.None
            };
        }

        #endregion
        
        #region move methodes

        private void OnMove()
        {
            //trouver une simplification a l'enterState
            _currentDir = _directionMode switch
            {
                LockDirectionMode.BLToTR when _inputDir is { x: > 0, z: > 0 } or { x: < 0, z: < 0 } => (Vector3.right * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                LockDirectionMode.TLToBR when _inputDir is { x: > 0, z: < 0 } or { x: < 0, z: > 0 } => (Vector3.forward * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                _ => Vector3.zero
            };

            _cc.SimpleMove(_currentDir * (_datasSo.moveDatasSo.moveSpeed * Constants.SpeedMultiplier * Time.deltaTime));
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