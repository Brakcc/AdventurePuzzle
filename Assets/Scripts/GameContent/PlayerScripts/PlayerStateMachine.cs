using System.Collections.Generic;
using GameContent.PlayerScripts.PlayerDatas;
using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.PlayerScripts
{
    [RequireComponent(typeof(CharacterController))]
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
                new LockedOnInterState(go),
                new LockedOnLeverState(go)
            };

            playerStatesDict = new Dictionary<string, AbstractPlayerState>
            {
                {"move", playerStates[0]},
                {"jump", playerStates[1]},
                {"interact", playerStates[2]},
                {"cancel", playerStates[3]},
                {"fall", playerStates[4]},
                {"locked", playerStates[5]},
                {"lever", playerStates[6]}
            };
            
            if (playerStates.Length == 0)
                return;

            var tempCc = GetComponent<CharacterController>();
            foreach (var state in playerStates)
            {
                state.SetCharaCont(tempCc);
                state.SetDatas(datasSo);
                state.SetChecker(checker);
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