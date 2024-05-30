using GameContent.Interactives.ClemInterTemplates;
using GameContent.Interactives.ClemInterTemplates.Levers;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class CancelState : AbstractPlayerState
    {
        #region constructor

        public CancelState(GameObject go, ControllerState state, PlayerStateMachine pM) : base(go, state, pM)
        {
        }

        #endregion
        
        #region methodes
        
        public override void OnEnterState()
        {
            _applyTimeCounter = _datasSo.interactDatasSo.applyTime;
        }

        public override void OnExitState()
        {
        }

        public override sbyte OnUpdate()
        {
            base.OnUpdate();
            
            SetCancelTime();
            GetOtherActionInputs();
            OnAction();
            
            SetCoyote();
            SetJumpBuffer();
            //OnInputVal();

            return 0;
        }

        #region cancel methodes

        private void SetCancelTime()
        {
            if (_datasSo.cancelInput.action.IsPressed())
            {
                _applyTimeCounter -= Time.deltaTime;
                return;
            }
            
            //_stateMachine.OnSwitchState("move");
            newStateMachine.SwitchState("move");
        }

        private void GetOtherActionInputs()
        {
            if (_datasSo.interactInput.action.WasPressedThisFrame() && _checker.InterRef is not null)
                //_stateMachine.OnSwitchState(_checker.InterRef is ReceptorInter or LeverInter ? "grab" : "interact");
                newStateMachine.SwitchState(_checker.InterRef is ReceptorInter or LeverInter ? "grab" : "interact");
        }
        
        private void OnAction()
        {
            if (_applyTimeCounter > 0)
                return;

            if (_checker.InterRef is not null)
            {
                if (_checker.InterRef is not EmitterInter && PlayerEnergyM.EnergyType != EnergyTypes.None ||
                    _checker.InterRef is EmitterInter { SourceCount: 0 } && PlayerEnergyM.EnergyType != EnergyTypes.None)
                {
                    PlayerEnergyM.CurrentSource.Source.InterAction();
                    PlayerEnergyM.CurrentSource = new SourceDatas();
                    PlayerEnergyM.OnSourceChangedDebug();
                }
                _checker.InterRef.PlayerCancel();
                //_stateMachine.OnSwitchState("move");
                newStateMachine.SwitchState("move");
                return;
            }

            if (_checker.InterRef is null && PlayerEnergyM.EnergyType != EnergyTypes.None)
            {
                PlayerEnergyM.CurrentSource.Source.InterAction();
                PlayerEnergyM.CurrentSource = new SourceDatas();
                PlayerEnergyM.OnSourceChangedDebug();
            }
            
            //_stateMachine.OnSwitchState("move");
            newStateMachine.SwitchState("move");
        }
        
        #endregion
        
        #region jumpSwitchers

        private void OnJump()
        {
            if ((!(_coyoteTimeCounter >= 0) || !_datasSo.jumpInput.action.IsPressed()) &&
                (!(_jumpBufferCounter >= 0) || !IsGrounded))
                return;
            
            //_stateMachine.OnSwitchState("jump");
            newStateMachine.SwitchState("jump");
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
                //_stateMachine.OnSwitchState("move");
                newStateMachine.SwitchState("move");
        }
        
        #endregion
        
        #endregion

        #region fields
        
        private float _applyTimeCounter;

        private float _coyoteTimeCounter;

        private float _jumpBufferCounter;

        #endregion
    }
}