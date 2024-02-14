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
            
            if (IsGrounded)
                _rb.drag = dragSpeed;
            else
                _rb.drag = 0;
        }

        protected override void OnFixedUpdate()
        {
            var trans = transform;
            
            _currentDir = trans.forward * _inputDir.x + trans.right * _inputDir.z;
            
            _rb.AddForce(_currentDir.normalized * (moveSpeed * 10), ForceMode.Force);
        }

        private void ClampVelocity()
        {
            _rb.velocity = new Vector3(ClampVelAxe(_rb.velocity.x),  _rb.velocity.y, ClampVelAxe(_rb.velocity.z));
        }

        private float ClampVelAxe(float vel) => Mathf.Clamp(vel, -moveSpeed, moveSpeed);
        
        #endregion
        
        #region fields

        private const float PlayerHeight = 1;
        
        [FieldCompletion] [SerializeField] private InputActionReference moveInput;
        
        [SerializeField] private float moveSpeed;

        [Space]
        [SerializeField] private float dragSpeed;
        
        [SerializeField] private LayerMask groundLayer;
        
        [SerializeField] private Transform orientation;

        private Vector3 _currentDir;

        private Vector3 _inputDir;

        private Rigidbody _rb;

        private bool IsGrounded => Physics.Raycast(transform.position, Vector3.down,
            PlayerHeight / 2 + Constants.GroundCheckSupLength, groundLayer);

        #endregion
    }
}