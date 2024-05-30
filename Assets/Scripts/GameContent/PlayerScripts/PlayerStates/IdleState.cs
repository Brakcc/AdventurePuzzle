﻿using GameContent.Interactives.ClemInterTemplates;
using GameContent.Interactives.ClemInterTemplates.Levers;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using GameContent.StateMachines;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class IdleState : AbstractPlayerState
    {
        #region constructor 
        
        public IdleState(GameObject go, ControllerState state, PlayerStateMachine pM) : base(go, state, pM)
        {
        }

        #endregion
        
        #region methodes

        public override void OnInit(GenericStateMachine m)
        {
            _lastDir = _isoForwardDir;
            base.OnInit(m);
        }
        
        public override void OnEnterState()
        {
            
        }

        public override void OnExitState()
        {
            
        }

        public override sbyte OnUpdate()
        {
            base.OnUpdate();
            
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _analogInputMagnitude = input.magnitude;
            _inputDir = new Vector3(input.x, 0, input.y).normalized;

            GatherInteractionInputs();

            OnFall();
            OnMove();

            return 0;
        }

        #region Move Switchers
        
        private void OnMove()
        {
            if (_analogInputMagnitude <= Constants.MinMoveInputValue)
                return;
            
            //_stateMachine.OnSwitchState("move");
            newStateMachine.SwitchState("move");
        }
        
        #endregion

        #region Fall Switchers
        
        private void OnFall()
        {
            if (!IsGrounded)
            {
                //_stateMachine.OnSwitchState("fall");
                newStateMachine.SwitchState("fall");
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
                        //_stateMachine.OnSwitchState("interact");
                        newStateMachine.SwitchState("interact");
                        return;
                    case ReceptorInter { IsMovable: true, CurrentEnergyType:EnergyTypes.Blue}:
                        //_stateMachine.OnSwitchState("grab");
                        newStateMachine.SwitchState("grab");
                        return;
                    case LeverInter : 
                        //_stateMachine.OnSwitchState("lever");
                        newStateMachine.SwitchState("lever");
                        return;
                    case not null:
                        //_stateMachine.OnSwitchState("interact");
                        newStateMachine.SwitchState("interact");
                        return;
                }
            }
            
            if (_datasSo.cancelInput.action.WasPressedThisFrame())
                //_stateMachine.OnSwitchState("cancel");
                newStateMachine.SwitchState("cancel");
        }

        #endregion
        
        #endregion

        #region fields
        
        private float _analogInputMagnitude;

        private Vector3 _lastDir;

        #endregion
    }
}