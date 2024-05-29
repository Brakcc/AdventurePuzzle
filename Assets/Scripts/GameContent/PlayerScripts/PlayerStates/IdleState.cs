using GameContent.Interactives.ClemInterTemplates;
using GameContent.Interactives.ClemInterTemplates.Levers;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class IdleState : AbstractPlayerState
    {
       #region constructor 
        
        public IdleState(GameObject go, ControllerState state) : base(go, state)
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
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            _stateMachine = null;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _analogInputMagnitude = input.magnitude;
            _inputDir = new Vector3(input.x, 0, input.y).normalized;

            GatherInteractionInputs();

            OnFall();
            OnMove();
        }

        #region Move Switchers
        
        private void OnMove()
        {
            if (_analogInputMagnitude <= Constants.MinMoveInputValue)
                return;
            
            _stateMachine.OnSwitchState("move");
        }
        
        #endregion

        #region Fall Switchers
        
        private void OnFall()
        {
            if (!IsGrounded)
            {
                _stateMachine.OnSwitchState("fall");
            }
        }
        
        #endregion
        
        #region Interact Switchers

        private void GatherInteractionInputs()
        {
            if (_datasSo.interactInput.action.WasPressedThisFrame())
            {
                switch (_checker.InterRef)
                {
                    case null:
                        _stateMachine.OnSwitchState("interact");
                        return;
                    case ReceptorInter { IsMovable: true, CurrentEnergyType:EnergyTypes.Blue}:
                        _stateMachine.OnSwitchState("locked");
                        return;
                    case LeverInter : 
                        _stateMachine.OnSwitchState("lever");
                        return;
                    case not null:
                        _stateMachine.OnSwitchState("interact");
                        return;
                }
            }
            
            if (_datasSo.cancelInput.action.WasPressedThisFrame())
                _stateMachine.OnSwitchState("cancel");
        }

        #endregion
        
        #endregion

        #region fields
        
        private float _analogInputMagnitude;

        private Vector3 _lastDir;

        #endregion
    }
}