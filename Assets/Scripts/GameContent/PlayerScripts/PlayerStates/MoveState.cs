using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class MoveState : BasePlayerState
    {
        #region methodes

        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            
            _coyoteTimeCounter = _datas.jumpDatas.coyoteTime;
            _jumpBufferCounter = Constants.SecuValuUnderZero;

            _rb.drag = _datas.groundingDatas.dragSpeed;
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            _stateMachine = null;
        }

        public override void OnUpdate()
        {
            var input = _datas.moveInput.action.ReadValue<Vector2>();
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
                
            _rb.AddForce(_currentDir.normalized * (_datas.moveDatas.moveSpeed * Constants.SpeedMultiplier), ForceMode.Acceleration);
        }
        
        private void ClampVelocity()
        {
            var vel = _rb.velocity;
            _rb.velocity = new Vector3(ClampSymmetric(vel.x, _datas.moveDatas.moveSpeed),  vel.y, ClampSymmetric(vel.z, _datas.moveDatas.moveSpeed));
        }
        
        #endregion

        #region jump checker

        private void OnJump()
        {
            if ((!(_coyoteTimeCounter >= 0) || !_datas.jumpInput.action.IsPressed()) &&
                (!(_jumpBufferCounter >= 0) || !IsGrounded))
                return;
            
            _stateMachine.OnSwitchState(_stateMachine.playerStates[1]);
        }

        private void SetCoyote()
        {
            if (IsGrounded)
                _coyoteTimeCounter = _datas.jumpDatas.coyoteTime;

            else
                _coyoteTimeCounter -= Time.deltaTime;

            _coyoteTimeCounter = Mathf.Clamp(_coyoteTimeCounter, Constants.SecuValuUnderZero, _datas.jumpDatas.coyoteTime);
        }

        private void SetJumpBuffer()
        {
            if (_datas.jumpInput.action.IsPressed())
                _jumpBufferCounter = _datas.jumpDatas.jumpBuffer;

            else
                _jumpBufferCounter -= Time.deltaTime;
            
            _jumpBufferCounter = Mathf.Clamp(_jumpBufferCounter, Constants.SecuValuUnderZero, _datas.jumpDatas.jumpBuffer);
        }
        
        #endregion
        
        #endregion
        
        #region fields

        private float _coyoteTimeCounter;

        private float _jumpBufferCounter;

        private bool IsGrounded => Physics.Raycast(transform.position, -transform.up,
            Constants.PlayerHeight / 2 + Constants.GroundCheckSupLength, _datas.groundingDatas.groundLayer);

        #endregion
    }
}