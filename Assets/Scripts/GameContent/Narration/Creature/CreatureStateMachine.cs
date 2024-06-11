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
        
        public bool IsSlower { get; set; }

        public bool IsDedge { get; set; }

        public byte CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                switch (_currentState)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        IsSlower = true;
                        animator.SetBool(IsMoving,  false);
                        break;
                    case 3:
                        IsDedge = true;
                        animator.SetBool(IsMoving,  false);
                        animator.SetBool(IsFatigue,  false);
                        animator.SetBool(IsDead, true); 
                        break;
                }
            }
        }
        
        #endregion

        #region methodes

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();

            _vertVelocity = 0;
            CurrentState = 0;
        }

        private void Update()
        {
            if (CurrentState is 0 or 3)
                return;
            
            SetAnims();
            
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

        private void OnRotate(Vector3 dir)
        {
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
        
        private void OnMove()
        {
            var tempDir = GetDir(playerRef.position);
            var tempVel = tempDir * creatureSpeed;
            
            _rb.velocity = new Vector3(tempVel.x, _rb.velocity.y, tempVel.z);
            
            OnRotate(tempDir);
        }
        
        private Vector3 GetDir(Vector3 targetPos)
        {
            var tempDir = targetPos - transform.position;
            var tempDist = tempDir.magnitude;
            
            return tempDir.normalized * creatureAccelerationCurve.Evaluate(tempDist);
        }

        #endregion

        #region anims

        private void SetAnims()
        {
            if (CurrentState != 3)
                animator.SetBool(!IsSlower ? IsMoving : IsFatigue, _rb.velocity.magnitude >= 1f);
        }

        public void SetAnims(string anim, bool state)
        {
            animator.SetBool(anim, state);
        }
        
        public void OnDie()
        {
            animator.SetBool(IsMoving, false);
            animator.SetBool(IsFatigue, false);
            animator.SetBool(IsDead, true);
        }

        #endregion
        
        #endregion

        #region fields

        [FieldCompletion(_checkedColor:FieldColor.Green)]
        [SerializeField] private Transform playerRef;

        [SerializeField] private Animator animator;
        
        [SerializeField] private AnimationCurve creatureAccelerationCurve;

        [SerializeField] private float creatureSpeed;
        
        [SerializeField] private float maxDistFromPlayer;

        [SerializeField] private float minDistFromPlayer;

        [SerializeField] private float groundCheckRayLength;

        [SerializeField] private LayerMask groundLayer;

        private Rigidbody _rb;

        private float _vertVelocity;

        private byte _currentState;
        
        private static readonly int IsMoving = Animator.StringToHash("isMoving");

        private static readonly int IsFatigue = Animator.StringToHash("isFatigue");
        
        private static readonly int IsDead = Animator.StringToHash("isDead");

        private const float Gravity = 9.81f;

        #endregion
    }
}