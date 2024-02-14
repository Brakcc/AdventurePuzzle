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
            _rb.freezeRotation = true;
        }

        protected override void OnUpdate()
        {
            var input = moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y);
            
            ClampVelocity();
            SetLerpedCoef();
            SetDrag();
        }

        protected override void OnFixedUpdate()
        {
            var trans = transform;
            var tempSpeed = Mathf.Lerp(0, moveSpeed, _currentSpeedLerpCoef);
            
            _currentDir = (trans.right * _inputDir.x + trans.forward * _inputDir.z).normalized;
            
            _rb.AddForce(_currentDir.normalized * (moveSpeed * Constants.SpeedMultiplier), ForceMode.Force); //move speed a 7.5
            //_rb.velocity = new Vector3(_currentDir.x, _rb.velocity.y, _currentDir.z) * tempSpeed; //move speed a 5
        }

        private void ClampVelocity()
        {
            var vel = _rb.velocity;
            _rb.velocity = new Vector3(ClampVelAxe(vel.x),  vel.y, ClampVelAxe(vel.z));
        }

        private float ClampVelAxe(float vel) => Mathf.Clamp(vel, -moveSpeed, moveSpeed);

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
        
        [FieldCompletion] [SerializeField] private InputActionReference moveInput;
        
        [SerializeField] private float moveSpeed;

        [Space]
        [SerializeField] private float dragSpeed;
        
        [SerializeField] private LayerMask groundLayer;

        private Vector3 _currentDir;

        private Vector3 _inputDir;

        private float _currentSpeedLerpCoef;

        private Rigidbody _rb;

        private bool IsGrounded => Physics.Raycast(transform.position, Vector3.down,
            PlayerHeight / 2 + Constants.GroundCheckSupLength, groundLayer);

        #endregion
    }
}