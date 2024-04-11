using System.Collections.Generic;
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
                new InteractState(go),
                new CancelState(go),
                new FallState(go),
                new LockedOnInterState(go)
            };

            playerStatesDict = new Dictionary<string, AbstractPlayerState>
            {
                {"move", new MoveState(go)},
                {"jump", new JumpState(go)},
                {"absorb", new InteractState(go)},
                {"apply", new CancelState(go)},
                {"fall", new FallState(go)},
                {"locked", new LockedOnInterState(go)}
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
        
        public void OnSwitchState(string newState)
        {
            _currentState.OnExitState(this);
            _currentState = playerStatesDict[newState];
            _currentState.OnEnterState(this);
        }
        
        #endregion
        
        #region fields
        
        public AbstractPlayerState[] playerStates;

        private Dictionary<string, AbstractPlayerState> playerStatesDict;
            
        [FieldCompletion] [SerializeField] protected BasePlayerDatasSO datasSo;

        [FieldCompletion] public InterCheckerState checker;
        
        private AbstractPlayerState _currentState;

        #endregion
    }
}