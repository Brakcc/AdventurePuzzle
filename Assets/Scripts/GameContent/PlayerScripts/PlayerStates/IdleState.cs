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
            _lastDir = IsoForwardDir;
            base.OnInit(m);
        }
        
        public override void OnEnterState()
        {
            AnimationManager.SetAnims("isWalking", false);
        }

        public override void OnExitState()
        {
        }

        public override sbyte OnUpdate()
        {
            OnCam();
            
            if (base.OnUpdate() == 1 || base.OnUpdate() == 0)
                return 2;
            
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
            
            stateMachine.SwitchState("move");
        }
        
        #endregion

        #region Fall Switchers
        
        private void OnFall()
        {
            if (!IsGrounded)
            {
                stateMachine.SwitchState("fall");
            }
        }
        
        #endregion
        
        #region Interact Switchers

        private void GatherInteractionInputs()
        {
            if (stateMachine == "camera")
                return;
            
            if (_datasSo.interactInput.action.WasPressedThisFrame())
            {
                switch (_checker.InterRef)
                {
                    case null:
                        stateMachine.SwitchState("interact");
                        return;
                    case ReceptorInter { IsMovable: true, CanSwitch: true, CurrentEnergyType:EnergyTypes.Blue}:
                        stateMachine.SwitchState("grab");
                        return;
                    case LeverInter:
                        stateMachine.SwitchState("lever");
                        return;
                    case EmitterInter:
                        if (PlayerEnergyM.EnergyType is not EnergyTypes.None)
                            _datasSo.interactDatasSo.OnVFX(0, _goRef.transform.position, 
                                                           _goRef.transform.rotation);
                        stateMachine.SwitchState("interact");
                        break;
                    case EnergySourceInter @ref:
                        if (@ref.IsActivated)
                            _datasSo.interactDatasSo.OnVFX(@ref.EnergyType is EnergyTypes.Green ? (byte)3 : (byte)4, _goRef.transform.position, 
                                                           _goRef.transform.rotation);
                        stateMachine.SwitchState("interact");
                        return;
                    case not null:
                        stateMachine.SwitchState("interact");
                        return;
                }
            }
            
            if (_datasSo.cancelInput.action.WasPressedThisFrame())
                stateMachine.SwitchState("cancel");
        }

        #endregion

        #region cam Switchers

        private void OnCam()
        {
            if (_datasSo.cameraInput.action.WasPressedThisFrame())
                stateMachine.SwitchState("camera");
        }

        #endregion
        
        #endregion

        #region fields
        
        private float _analogInputMagnitude;

        private Vector3 _lastDir;

        #endregion
    }
}