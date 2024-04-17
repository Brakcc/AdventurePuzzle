using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class MoveState : AbstractPlayerState
    {
        #region constructor

        public MoveState(GameObject go) : base(go)
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
            
            _coyoteTimeCounter = _datasSo.jumpDatasSo.coyoteTime;
            _jumpBufferCounter = Constants.SecuValuUnderZero;

            _rb.drag = _datasSo.groundingDatasSo.dragSpeed;
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            _stateMachine = null;
        }

        public override void OnUpdate()
        {
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y).normalized;
            
            GetInteractInputs();
            
            //Jump
            SetCoyote();
            SetJumpBuffer();
            
            //Fall
            OnFall();
        }

        public override void OnFixedUpdate()
        {
            ClampVelocity();
            
            OnMove();
            OnRotate();
            //OnJump();
        }

        #region rotation mathodes

        private void OnRotate()
        {
            if (_inputDir.magnitude <= Constants.MinMoveInputValue)
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
            _currentDir = (_isoRightDir * _inputDir.x + _isoForwardDir * _inputDir.z).normalized;
                
            _rb.AddForce(_currentDir.normalized * (_datasSo.moveDatasSo.moveSpeed * Constants.SpeedMultiplier), ForceMode.Acceleration);
            //_rb.position += _currentDir.normalized * (_datasSo.moveDatasSo.moveSpeed * Constants.SpeedMultiplier * Time.deltaTime);
        }
        
        private void ClampVelocity()
        {
            var vel = _rb.velocity;
            _rb.velocity = new Vector3(ClampSymmetric(vel.x, _datasSo.moveDatasSo.moveSpeed),  vel.y, ClampSymmetric(vel.z, _datasSo.moveDatasSo.moveSpeed));
        }
        
        #endregion

        #region jump Switchers

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

        #region interact Switchers

        private void GetInteractInputs()
        {
            if (_datasSo.interactInput.action.WasPressedThisFrame())
                _stateMachine.OnSwitchState(_stateMachine.playerStates[2]);
            
            if (_datasSo.cancelInput.action.WasPressedThisFrame())
                _stateMachine.OnSwitchState(_stateMachine.playerStates[3]);
        }

        #endregion

        #region fall Switchers

        private void OnFall()
        {
            if (!IsGrounded)
                _stateMachine.OnSwitchState("fall");
        }

        #endregion
        
        #endregion
        
        #region fields

        private float _coyoteTimeCounter;

        private float _jumpBufferCounter;

        private Vector3 _lastDir;

        #endregion
    }
}