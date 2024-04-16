﻿using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class CancelState : AbstractPlayerState
    {
        #region constructor

        public CancelState(GameObject go) : base(go)
        {
        }

        #endregion
        
        #region methodes
        
        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _checker = _stateMachine.checker;

            _rb.drag = _datasSo.groundingDatasSo.dragSpeed;

            _applyTimeCounter = _datasSo.interactDatasSo.applyTime;
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
        }

        public override void OnUpdate()
        {
            SetApplyTime();
            GetOtherActionInputs();
            OnAction();
            
            SetCoyote();
            SetJumpBuffer();
            //OnInputVal();
        }

        public override void OnFixedUpdate()
        {
            //OnJump();
        }

        #region apply methodes

        private void SetApplyTime()
        {
            if (_datasSo.cancelInput.action.IsPressed())
            {
                _applyTimeCounter -= Time.deltaTime;
                return;
            }
            
            _stateMachine.OnSwitchState(_stateMachine.playerStates[0]);
        }

        private void GetOtherActionInputs()
        {
            if (_datasSo.interactInput.action.WasPressedThisFrame())
                _stateMachine.OnSwitchState("interact");
        }
        
        private void OnAction()
        {
            if (_applyTimeCounter > 0)
                return;
            
            if (_checker.InterRef != null)
                _checker.InterRef.PlayerCancel();
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

        private InterCheckerState _checker;
        
        private float _applyTimeCounter;

        private float _coyoteTimeCounter;

        private float _jumpBufferCounter;

        #endregion
    }
}