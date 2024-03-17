using GameContent.PlayerScripts.PlayerDatas;
using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.PlayerScripts
{
    public class PlayerStateMachine : MonoBehaviour
    {
        #region methodes

        private void Awake()
        {
            var go = gameObject;
            playerStates = new AbstractPlayerState[]
            {
                new MoveState(go),
                new JumpState(go),
                new AbsorbState(go),
                new ApplyState(go),
                new FallState(go)
            };
            
            if (playerStates.Length == 0)
                return;

            var tempRb = GetComponent<Rigidbody>();
            foreach (var state in playerStates)
            {
                state.SetRigidBody(tempRb);
                state.SetDatas(datasSo);
                state.OnInit();
            }
            
            _currentState = playerStates[0];
        }

        private void Start()
        {
            if (playerStates.Length == 0)
                return;
            
            _currentState.OnEnterState(this);
        }

        private void Update()
        {
            _currentState.OnUpdate();
        }

        private void FixedUpdate()
        {
            _currentState.OnFixedUpdate();
        }

        public void OnSwitchState(AbstractPlayerState newState)
        {
            _currentState.OnExitState(this);
            _currentState = newState;
            _currentState.OnEnterState(this);
        }
        
        #endregion
        
        #region fields
        
        [HideInInspector] public AbstractPlayerState[] playerStates;
        
        [FieldCompletion] [SerializeField] protected BasePlayerDatasSO datasSo;

        private AbstractPlayerState _currentState;

        #endregion
    }
}