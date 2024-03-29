﻿using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class JumpState : AbstractPlayerState
    {
        #region methodes
        
        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            
            _rb.drag = 0;
            _jumpTimer = Constants.JumpTimerAfterInput;
            
            var vel = _rb.velocity;
            vel = new Vector3(vel.x, 0, vel.z);
            _rb.velocity = vel;
            
            _rb.AddForce(transform.up * _datasSo.jumpDatasSo.jumpForce, ForceMode.Impulse);
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            _stateMachine = null;
        }

        public override void OnUpdate()
        {
            GetInputVal();
            GetJumpTimer();
        }

        public override void OnFixedUpdate()
        {
            OnMove();
            OnLand();
            ClampVelocity();
        }

        #region move methodes

        private void GetInputVal()
        {
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y).normalized;
        }
        
        private void OnMove()
        {
            _currentDir = (Vector3.right * _inputDir.x + Vector3.forward * _inputDir.z).normalized;
                
            _rb.AddForce(_currentDir.normalized * (_datasSo.moveDatasSo.moveSpeed * Constants.SpeedMultiplier * _datasSo.jumpDatasSo.airControlCoef), ForceMode.Acceleration);
        }
        
        private void ClampVelocity()
        {
            var vel = _rb.velocity;
            _rb.velocity = new Vector3(ClampSymmetric(vel.x, _datasSo.moveDatasSo.moveSpeed),  vel.y, ClampSymmetric(vel.z, _datasSo.moveDatasSo.moveSpeed));
        }

        #endregion

        #region checkers

        private void GetJumpTimer()
        {
            _jumpTimer -= Time.deltaTime;

            _jumpTimer = Mathf.Clamp(_jumpTimer, Constants.SecuValuUnderZero, Constants.JumpTimerAfterInput);
        }
        
        private void OnLand()
        {
            if (_jumpTimer <= 0 && IsGrounded)
                _stateMachine.OnSwitchState(_stateMachine.playerStates[0]);
        }

        #endregion
        
        #endregion

        #region fields

        private float _jumpTimer;

        private bool IsGrounded => Physics.Raycast(transform.position, -transform.up,
            Constants.PlayerHeight / 2 + Constants.GroundCheckSupLength, _datasSo.groundingDatasSo.groundLayer);

        #endregion
    }
}