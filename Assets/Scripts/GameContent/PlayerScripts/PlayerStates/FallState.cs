using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class FallState : AbstractPlayerState
    {
        #region constructor

        public FallState(GameObject go) : base(go)
        {
        }

        #endregion
        
        #region methodes
        
        public override void OnInit()
        {
            _lastDir = _isoForwardDir;
            base.OnInit();
        }
        
        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _rb.drag = _datasSo.groundingDatasSo.dragSpeed;
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            _lerpCoef = 0;
            _stateMachine = null;
        }
        
        public override void OnUpdate()
        {
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y).normalized;
            
            OnGrounded();
            SetLerpCoef();
        }

        public override void OnFixedUpdate()
        {
            ClampVelocity();
            
            OnMove();
            OnRotate();
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
            _currentDir = (_isoRightDir * _inputDir.x + _isoForwardDir * _inputDir.z).normalized;
            _rb.AddForce(_currentDir.normalized * (_datasSo.moveDatasSo.moveSpeed * Constants.SpeedMultiplier * Constants.FallSpeedMultiplier), ForceMode.Acceleration);

            _rb.velocity = new Vector3(_rb.velocity.x, Mathf.Lerp(0, _datasSo.fallDatasSo.fallSpeed, _lerpCoef),_rb.velocity.z);
        }
        
        private void ClampVelocity()
        {
            var vel = _rb.velocity;
            _rb.velocity = new Vector3(ClampSymmetric(vel.x, _datasSo.moveDatasSo.moveSpeed),  vel.y, ClampSymmetric(vel.z, _datasSo.moveDatasSo.moveSpeed));
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
                _stateMachine.OnSwitchState("move");
        }

        #endregion
        
        #endregion

        #region fields

        private Vector3 _lastDir;

        private float _lerpCoef;

        #endregion
    }
}