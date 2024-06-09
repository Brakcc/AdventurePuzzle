using GameContent.Interactives.ClemInterTemplates;
using GameContent.Interactives.ClemInterTemplates.Levers;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using GameContent.StateMachines;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class MoveState : AbstractPlayerState
    {
        #region constructor

        public MoveState(GameObject go, ControllerState state, PlayerStateMachine pM) : base(go, state, pM)
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
            _coyoteTimeCounter = _datasSo.jumpDatasSo.coyoteTime;
            _jumpBufferCounter = Constants.SecuValuUnderZero;
            
            AnimationManager.SetAnims("isWalking", true);
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
            
            GetInteractInputs();
            OnCam();
            
            //Jump
            SetCoyote();
            SetJumpBuffer();
            
            //Fall
            OnFall();

            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            base.OnFixedUpdate();
            OnMove();
            OnRotate();
            //OnJump();
            
            return 0;
        }

        #region rotation methodes

        private void OnRotate()
        {
            if (_analogInputMagnitude <= Constants.MinMoveInputValue)
                return;
            
            var angle = Vector3.Dot(_lastDir, _currentDir) / (_currentDir.magnitude * _lastDir.magnitude);
            if (Mathf.Acos(angle) > Constants.MinPlayerRotationAngle)
            {
                _currentDir = Vector3.MoveTowards(_currentDir, _lastDir, _datasSo.moveDatasSo.rotaSpeedCoef * Time.fixedDeltaTime);
            }
            
            _goRef.transform.rotation = Quaternion.LookRotation(_currentDir);
        }

        #endregion

        #region move methodes

        private void OnMove()
        {
            if (_analogInputMagnitude <= Constants.MinMoveInputValue)
                _idleSwitchCounter += Time.deltaTime;

            if (_idleSwitchCounter > Constants.MinMoveInputValue)
            {
                _idleSwitchCounter = 0;
                stateMachine.SwitchState("idle");
            }
            
            _currentDir = (IsoRightDir * _inputDir.x + IsoForwardDir * _inputDir.z).normalized;
            _cc.SimpleMove(_currentDir.normalized * (_datasSo.moveDatasSo.moveSpeed * Constants.SpeedMultiplier * Time.deltaTime));
        }
        
        #endregion

        #region jump Switchers

        private void OnJump()
        {
            if ((!(_coyoteTimeCounter >= 0) || !_datasSo.jumpInput.action.IsPressed()) &&
                (!(_jumpBufferCounter >= 0) || !IsGrounded))
                return;
            
            //_stateMachine.OnSwitchState("jump");
            stateMachine.SwitchState("jump");
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

        #region interact Switchers

        private void GetInteractInputs()
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
        
        #region fall Switchers

        private void OnFall()
        {
            if (!IsGrounded)
            {
                //_stateMachine.OnSwitchState("fall");
                stateMachine.SwitchState("fall");
            }
        }

        #endregion
        
        #endregion
        
        #region fields

        private float _coyoteTimeCounter;

        private float _jumpBufferCounter;

        private float _analogInputMagnitude;

        private float _idleSwitchCounter;
        
        private Vector3 _lastDir;

        #endregion
    }
}
