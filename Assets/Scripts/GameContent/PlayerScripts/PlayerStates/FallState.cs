using GameContent.StateMachines;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class FallState : AbstractPlayerState
    {
        #region constructor

        public FallState(GameObject go, ControllerState state, PlayerStateMachine pM) : base(go, state, pM)
        {
        }

        #endregion
        
        #region methodes
        
        public override void OnInit(GenericStateMachine m)
        {
            _lastDir = IsoForwardDir;
            base.OnInit(m);
        }

        public override void OnEnterState()
        {
            AnimationManager.SetAnims("isFalling", true);
        }

        public override void OnExitState()
        {
            AnimationManager.SetAnims("isFalling", false);
            _lerpCoef = 0;
        }

        public override sbyte OnUpdate()
        {
            base.OnUpdate();
            
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y).normalized;
            
            OnGrounded();
            SetLerpCoef();

            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            base.OnFixedUpdate();
            OnMove();
            OnRotate();

            return 0;
        }

        #region rotation mathodes

        private void OnRotate()
        {
            if (_inputDir.magnitude <= Constants.MinMoveInputValue)
                return;
            
            var angle = Vector3.Dot(_lastDir, _currentDir) / (_currentDir.magnitude * _lastDir.magnitude);
            if (Mathf.Acos(angle) > Constants.MinPlayerRotationAngle)
            {
                _currentDir = Vector3.MoveTowards(_currentDir, _lastDir, _datasSo.moveDatasSo.rotaSpeedCoef * Time.fixedDeltaTime);
            }
            
            _goRef.transform.rotation = Quaternion.LookRotation(_currentDir);
        }
        
        #endregion
        
        #region move methodes

        private void OnMove()
        {
            _currentDir = (IsoRightDir * _inputDir.x + IsoForwardDir * _inputDir.z).normalized;

            _cc.SimpleMove(_currentDir.normalized * (_datasSo.moveDatasSo.moveSpeed * Constants.SpeedMultiplier * Time.deltaTime * 0.5f));
            _cc.Move(Vector3.down * (Mathf.Lerp(0, _datasSo.fallDatasSo.fallSpeed, _lerpCoef) * Time.deltaTime));
        }

        private void SetLerpCoef()
        {
            _lerpCoef += Time.deltaTime;
        }
        
        #endregion

        #region move Switchers

        private void OnGrounded()
        {
            if (IsGrounded)
                stateMachine.SwitchState("move");
        }

        #endregion
        
        #endregion

        #region fields

        private Vector3 _lastDir;

        private float _lerpCoef;

        #endregion
    }
}