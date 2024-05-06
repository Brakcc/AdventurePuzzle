using GameContent.Interactives.ClemInterTemplates;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class InteractState : AbstractPlayerState
    {
        #region constructor

        public InteractState(GameObject go) : base(go)
        {
        }

        #endregion
        
        #region methodes
        
        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _absorbTimeCounter = _datasSo.interactDatasSo.absorbTime;
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            SetInteractTime();
            GetOtherActionInput();
            OnAction();
            
            SetCoyote();
            SetJumpBuffer();
            //OnInputVal();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            //OnJump();
        }

        #region absorb methodes

        private void SetInteractTime()
        {
            if (_datasSo.interactInput.action.IsPressed())
            {
                _absorbTimeCounter -= Time.deltaTime;
                return;
            }
            
            _stateMachine.OnSwitchState(_stateMachine.playerStates[0]);
        }

        private void GetOtherActionInput()
        {
            if (_datasSo.cancelInput.action.WasPressedThisFrame())
                _stateMachine.OnSwitchState("cancel");
        }
        
        private void OnAction()
        {
            if (_absorbTimeCounter > 0)
                return;

            if (_checker.InterRef is null or ReceptorInter)
            {
                _stateMachine.OnSwitchState(_stateMachine.playerStates[0]);
                return;
            }

            _checker.InterRef.PlayerAction();
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
        
        private float _absorbTimeCounter;

        private float _coyoteTimeCounter;

        private float _jumpBufferCounter;

        #endregion
    }
}