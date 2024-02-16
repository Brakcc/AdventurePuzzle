using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.CustomAttributes;

namespace GameContent.PlayerScripts.PlayerStates
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveState : BasePlayerState
    {
        #region methodes

        protected override void OnStart()
        {
            _rb = GetComponent<Rigidbody>();

            _coyoteTimeCounter = coyoteTime;
            _jumpBufferCounter = Constants.SecuAValuUnderZero;
        }

        protected override void OnUpdate()
        {
            var input = moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y);
            
            //Moves
            ClampVelocity();
            SetLerpedCoef();
            SetDrag();
            
            //Jump
            SetCoyote();
            SetJumpBuffer();
        }

        protected override void OnFixedUpdate()
        {
            OnMove();
            OnJump();
        }

        #region move methodes

        private void OnMove()
        {
            _currentDir = (Vector3.right * _inputDir.x + Vector3.forward * _inputDir.z).normalized;
            
            if (IsGrounded)
                _rb.AddForce(_currentDir.normalized * (moveSpeed * Constants.SpeedMultiplier), ForceMode.Acceleration);
            
            else
                _rb.AddForce(_currentDir.normalized * (moveSpeed * Constants.SpeedMultiplier * airControlCoef), ForceMode.Acceleration);
        }
        
        private void ClampVelocity()
        {
            var vel = _rb.velocity;
            _rb.velocity = new Vector3(ClampSymmetric(vel.x, moveSpeed),  vel.y, ClampSymmetric(vel.z, moveSpeed));
        }

        private static float ClampSymmetric(float val, float clamper) => Mathf.Clamp(val, -clamper, clamper);
        
        #endregion

        #region jump methodes

        private void OnJump()
        {
            if ((!(_coyoteTimeCounter >= 0) || !jumpInput.action.IsPressed()) &&
                (!(_jumpBufferCounter >= 0) || !IsGrounded))
                return;
            
            var vel = _rb.velocity;
            vel = new Vector3(vel.x, 0, vel.z);
            _rb.velocity = vel;
            
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void SetCoyote()
        {
            if (IsGrounded)
                _coyoteTimeCounter = coyoteTime;

            else
                _coyoteTimeCounter -= Time.deltaTime;

            _coyoteTimeCounter = Mathf.Clamp(_coyoteTimeCounter, Constants.SecuAValuUnderZero, coyoteTime);
        }

        private void SetJumpBuffer()
        {
            if (jumpInput.action.IsPressed())
                _jumpBufferCounter = jumpBuffer;

            else
                _jumpBufferCounter -= Time.deltaTime;
            
            _jumpBufferCounter = Mathf.Clamp(_jumpBufferCounter, Constants.SecuAValuUnderZero, jumpBuffer);
        }
        
        #endregion
        
        #region values setters
        
        private void SetLerpedCoef()
        {
            if (_inputDir.magnitude >= Constants.MinMoveInputValue)
                _currentSpeedLerpCoef += Time.deltaTime * 5;
            else
                _currentSpeedLerpCoef -= Time.deltaTime * 5;

            _currentSpeedLerpCoef = Mathf.Clamp(_currentSpeedLerpCoef, 0, 1);
        }

        private void SetDrag()
        {
            if (IsGrounded)
                _rb.drag = dragSpeed;
            else
                _rb.drag = 0;
        }
        
        #endregion
        
        #endregion
        
        #region fields

        private const float PlayerHeight = 2;
        [Header("Inputs")]
        [FieldCompletion] [SerializeField] private InputActionReference moveInput;

        [FieldCompletion] [SerializeField] private InputActionReference jumpInput;
        
        [Header("Move")]
        [SerializeField] private float moveSpeed;
        
        [SerializeField] private float dragSpeed;
        
        [SerializeField] private LayerMask groundLayer;
        
        private Vector3 _currentDir;
        
        private Vector3 _inputDir;
        
        private float _currentSpeedLerpCoef;

        [Space] [Header("Jump")]
        [SerializeField] private float jumpForce;

        [SerializeField] private float coyoteTime;

        private float _coyoteTimeCounter;

        [SerializeField] private float jumpBuffer;

        private float _jumpBufferCounter;

        [FieldColorLerp(0, 1)] [Range(0, 1)] [SerializeField] private float airControlCoef;

        private Rigidbody _rb;

        private bool IsGrounded => Physics.Raycast(transform.position, -transform.up,
            PlayerHeight / 2 + Constants.GroundCheckSupLength, groundLayer);

        #endregion
    }
}