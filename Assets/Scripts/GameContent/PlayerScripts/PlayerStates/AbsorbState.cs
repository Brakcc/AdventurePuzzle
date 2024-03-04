using System;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class AbsorbState : AbstractPlayerState
    {
        #region methodes
        
        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _rb.drag = _datasSo.groundingDatasSo.dragSpeed;

            _absorbTimeCounter = _datasSo.interactDatasSo.absorbTime;
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
        }

        public override void OnUpdate()
        {
            SetAbsorbTime();
            GetOtherActionInput();
            OnAction();
            
            SetCoyote();
            SetJumpBuffer();
            //OnInputVal();
        }

        public override void OnFixedUpdate()
        {
            OnJump();
        }

        #region absorb methodes

        private void SetAbsorbTime()
        {
            if (_datasSo.absorbInput.action.IsPressed())
            {
                _absorbTimeCounter -= Time.deltaTime;
                return;
            }
            
            _stateMachine.OnSwitchState(_stateMachine.playerStates[0]);
        }

        private void GetOtherActionInput()
        {
            if (_datasSo.applyInput.action.WasPressedThisFrame())
                _stateMachine.OnSwitchState(_stateMachine.playerStates[3]);
        }
        
        private void OnAction()
        {
            if (_absorbTimeCounter > 0)
                return;
            
            OnAbsorb?.Invoke();
            _stateMachine.OnSwitchState(_stateMachine.playerStates[0]);
        }
        
        #endregion
        
        #region jumpSwitchers

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
        
        #region moveSwitchers

        private void OnInputVal()
        {
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
           
            if (input.magnitude >= Constants.MinMoveInputValue)
                _stateMachine.OnSwitchState(_stateMachine.playerStates[0]);
        }
        
        #endregion
        
        #endregion

        #region fields

        public static event Action OnAbsorb;

        private float _absorbTimeCounter;

        private float _coyoteTimeCounter;

        private float _jumpBufferCounter;
        
        private bool IsGrounded => Physics.Raycast(transform.position, -transform.up,
            Constants.PlayerHeight / 2 + Constants.GroundCheckSupLength, _datasSo.groundingDatasSo.groundLayer);

        #endregion
    }
}