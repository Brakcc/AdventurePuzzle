using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class MoveState : AbstractPlayerState
    {
        #region methodes

        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            
            _coyoteTimeCounter = _datasSo.jumpDatasSo.coyoteTime;
            _jumpBufferCounter = Constants.SecuValuUnderZero;

            _rb.drag = _datasSo.groundingDatasSo.dragSpeed;
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            _stateMachine = null;
        }

        public override void OnUpdate()
        {
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y).normalized;
            
            //Jump
            SetCoyote();
            SetJumpBuffer();
        }

        public override void OnFixedUpdate()
        {
            ClampVelocity();
            
            OnMove();
            OnJump();
        }

        #region move methodes

        private void OnMove()
        {
            _currentDir = (Vector3.right * _inputDir.x + Vector3.forward * _inputDir.z).normalized;
                
            _rb.AddForce(_currentDir.normalized * (_datasSo.moveDatasSo.moveSpeed * Constants.SpeedMultiplier), ForceMode.Acceleration);
        }
        
        private void ClampVelocity()
        {
            var vel = _rb.velocity;
            _rb.velocity = new Vector3(ClampSymmetric(vel.x, _datasSo.moveDatasSo.moveSpeed),  vel.y, ClampSymmetric(vel.z, _datasSo.moveDatasSo.moveSpeed));
        }
        
        #endregion

        #region jump checker

        private void OnJump()
        {
            if ((!(_coyoteTimeCounter >= 0) || !_datasSo.jumpInput.action.IsPressed()) &&
                (!(_jumpBufferCounter >= 0) || !IsGrounded))
                return;
            
            _stateMachine.OnSwitchState(_stateMachine.playerStates[1]);
        }

        private void SetCoyote()
        {
            if (IsGrounded)
                _coyoteTimeCounter = _datasSo.jumpDatasSo.coyoteTime;

            else
                _coyoteTimeCounter -= Time.deltaTime;

            _coyoteTimeCounter = Mathf.Clamp(_coyoteTimeCounter, Constants.SecuValuUnderZero, _datasSo.jumpDatasSo.coyoteTime);
        }

        private void SetJumpBuffer()
        {
            if (_datasSo.jumpInput.action.IsPressed())
                _jumpBufferCounter = _datasSo.jumpDatasSo.jumpBuffer;

            else
                _jumpBufferCounter -= Time.deltaTime;
            
            _jumpBufferCounter = Mathf.Clamp(_jumpBufferCounter, Constants.SecuValuUnderZero, _datasSo.jumpDatasSo.jumpBuffer);
        }
        
        #endregion
        
        #endregion
        
        #region fields

        private float _coyoteTimeCounter;

        private float _jumpBufferCounter;

        private bool IsGrounded => Physics.Raycast(transform.position, -transform.up,
            Constants.PlayerHeight / 2 + Constants.GroundCheckSupLength, _datasSo.groundingDatasSo.groundLayer);

        #endregion
    }
}