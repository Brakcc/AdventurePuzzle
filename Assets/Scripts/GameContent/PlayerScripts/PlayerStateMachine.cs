using System.Collections.Generic;
using GameContent.PlayerScripts.PlayerDatas;
using GameContent.PlayerScripts.PlayerStates;
using GameContent.PlayerScripts.PlayerStates.ForcedStates;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.PlayerScripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerStateMachine : MonoBehaviour
    {
        #region properties

        public ControllerState CurrentState { get; set; }

        #endregion
        
        #region methodes

        private void Awake()
        {
            var go = gameObject;
            
            playerStates = new AbstractPlayerState[]
            {
                new IdleState(go, ControllerState.idle),
                new MoveState(go, ControllerState.move),
                new JumpState(go, ControllerState.jump),
                new InteractState(go, ControllerState.interact),
                new CancelState(go, ControllerState.cancel),
                new FallState(go, ControllerState.fall),
                new LockedOnInterState(go, ControllerState.locked),
                new LockedOnLeverState(go, ControllerState.lever),
                new CameraFocusState(go, ControllerState.camera),
                new CinematicIdleForcedState(go, ControllerState.cineIdle),
                new CinematicMoveForcedState(go, ControllerState.cineMove)
            };

            playerStatesDict = new Dictionary<string, AbstractPlayerState>
            {
                {"idle", playerStates[0]},
                {"move", playerStates[1]},
                {"jump", playerStates[2]},
                {"interact", playerStates[3]},
                {"cancel", playerStates[4]},
                {"fall", playerStates[5]},
                {"locked", playerStates[6]},
                {"lever", playerStates[7]},
                {"camera", playerStates[8]},
                {"cineIdle", playerStates[9]},
                {"cineMove", playerStates[10]}
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
            OnSwitchState(playerStatesDict[newState]);
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