using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Narration.Creature
{
    //Bon en fait c'est pas une state machine parce que flemme aussi la
    [RequireComponent(typeof(Rigidbody))]
    public class CreatureStateMachine : MonoBehaviour
    {
        #region properties

        private bool IsGrounded => Physics.Raycast(transform.position, Vector3.down, groundCheckRayLength, groundLayer);

        #endregion

        #region methodes

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();

            _vertVelocity = 0;
        }

        private void Update()
        {
            //Plan Move
            SecuTeleport();
            OnMove();
        }

        private void FixedUpdate()
        {
            //Gravity
            SetGravity();
            SetVertPos();
        }

        #region Vert Pos

        private void SetVertPos()
        {
            var velocity = _rb.velocity;
            _rb.velocity = IsGrounded ? new Vector3(velocity.x, 0, velocity.z) : new Vector3(velocity.x, _vertVelocity, velocity.z);
        }
        
        private void SetGravity()
        {
            if (!IsGrounded)
                _vertVelocity -= Gravity * Time.deltaTime;
            else
                _vertVelocity = 0;

            _vertVelocity = Mathf.Clamp(_vertVelocity, -15, 15);
        }

        #endregion

        #region Movement
        
        private void SecuTeleport()
        {
            var tempDir = playerRef.position - transform.position;
            var tempDist = tempDir.magnitude;
            
            if (tempDist < maxDistFromPlayer)
                return;

            _rb.position = playerRef.position - tempDir.normalized * minDistFromPlayer;
            _rb.velocity = Vector3.zero;
            _vertVelocity = 0;
            SetGravity();
            SetVertPos();
        }

        private void OnMove()
        {
            var tempDir = GetDir(playerRef.position);
            var tempVel = tempDir * creatureSpeed;
            
            _rb.velocity = new Vector3(tempVel.x, _rb.velocity.y, tempVel.z);
        }
        
        private Vector3 GetDir(Vector3 targetPos)
        {
            var tempDir = targetPos - transform.position;
            var tempDist = tempDir.magnitude;
            
            return tempDir.normalized * creatureAccelerationCurve.Evaluate(tempDist);
        }

        #endregion
        
        #endregion

        #region fields

        [FieldCompletion(_checkedColor:FieldColor.Green)]
        [SerializeField] private Transform playerRef;
        
        [SerializeField] private AnimationCurve creatureAccelerationCurve;

        [SerializeField] private float creatureSpeed;
        
        [SerializeField] private float maxDistFromPlayer;

        [SerializeField] private float minDistFromPlayer;

        [SerializeField] private float groundCheckRayLength;

        [SerializeField] private LayerMask groundLayer;

        private Rigidbody _rb;

        private float _vertVelocity;

        private const float Gravity = 9.81f;

        #endregion
    }
}