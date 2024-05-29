using System;
using System.Collections.Generic;
using GameContent.PlayerScripts.PlayerDatas;
using GameContent.PlayerScripts.PlayerStates;
using GameContent.PlayerScripts.PlayerStates.ForcedStates;
using GameContent.StateMachines;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.PlayerScripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerStateMachine : MonoBehaviour
    {
        #region properties

        public ControllerState CurrentState { get; set; }

        public GenericStateMachine Machine => _stateMachine;

        #endregion
        
        #region methodes

        private void Awake()
        {
            _stateMachine = new GenericStateMachine(11);
            var go = gameObject;

            pSD = new Dictionary<string, AbstractPlayerState>
            {
                {"idle", new IdleState(go, ControllerState.idle)},
                {"move", new MoveState(go, ControllerState.move)},
                {"jump", new JumpState(go, ControllerState.jump)},
                {"interact", new InteractState(go, ControllerState.interact)},
                {"cancel", new CancelState(go, ControllerState.cancel)},
                {"fall", new FallState(go, ControllerState.fall)},
                {"locked", new LockedOnInterState(go, ControllerState.locked)},
                {"lever", new LockedOnLeverState(go, ControllerState.lever)},
                {"camera", new CameraFocusState(go, ControllerState.camera)},
                {"cineIdle", new CinematicIdleForcedState(go, ControllerState.cineIdle)},
                {"cineMove", new CinematicMoveForcedState(go, ControllerState.cineMove)}
            };
            
            _stateMachine.SetCallBacks((int)pSD["idle"].StateFlag, pSD["idle"].OnInit, pSD["idle"].OnEnterState, pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState);
            _stateMachine.SetCallBacks((int)pSD["move"].StateFlag, pSD["move"].OnInit, pSD["move"].OnEnterState, pSD["move"].OnUpdate, pSD["move"].OnFixedUpdate, pSD["move"].OnExitState);
            _stateMachine.SetCallBacks((int)pSD["jump"].StateFlag, pSD["jump"].OnInit, pSD["jump"].OnEnterState, pSD["jump"].OnUpdate, pSD["jump"].OnFixedUpdate, pSD["jump"].OnExitState);
            _stateMachine.SetCallBacks((int)pSD["idle"].StateFlag, pSD["idle"].OnInit, pSD["idle"].OnEnterState, pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState);
            _stateMachine.SetCallBacks((int)pSD["idle"].StateFlag, pSD["idle"].OnInit, pSD["idle"].OnEnterState, pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState);
            _stateMachine.SetCallBacks((int)pSD["idle"].StateFlag, pSD["idle"].OnInit, pSD["idle"].OnEnterState, pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState);
            _stateMachine.SetCallBacks((int)pSD["idle"].StateFlag, pSD["idle"].OnInit, pSD["idle"].OnEnterState, pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState);
            _stateMachine.SetCallBacks((int)pSD["idle"].StateFlag, pSD["idle"].OnInit, pSD["idle"].OnEnterState, pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState);
            _stateMachine.SetCallBacks((int)pSD["idle"].StateFlag, pSD["idle"].OnInit, pSD["idle"].OnEnterState, pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState);
            _stateMachine.SetCallBacks((int)pSD["idle"].StateFlag, pSD["idle"].OnInit, pSD["idle"].OnEnterState, pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState);
            _stateMachine.SetCallBacks((int)pSD["idle"].StateFlag, pSD["idle"].OnInit, pSD["idle"].OnEnterState, pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState);
            
            if (pSD.Count == 0)
                return;

            var tempCc = GetComponent<CharacterController>();
            foreach (var state in pSD.Values)
            {
                state.SetStateMachine(this);
                state.SetCharaCont(tempCc);
                state.SetDatas(datasSo);
                state.SetChecker(checker);
                state.OnInit(_stateMachine);
            }
            
            _currentState = pSD["idle"];
        }

        private void Start()
        {
            if (pSD.Count == 0)
                return;
            
            _currentState.OnEnterState();
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
            _currentState.OnExitState();
            _currentState = newState;
            _currentState.OnEnterState();
        }
        
        public void OnSwitchState(string newState)
        {
            OnSwitchState(pSD[newState]);
        }
        
        #endregion
        
        #region fields
        
        [FieldCompletion] [SerializeField] protected BasePlayerDatasSO datasSo;

        [FieldCompletion] public InterCheckerState checker;

        private GenericStateMachine _stateMachine;

        private Dictionary<string, AbstractPlayerState> pSD; //playerStateDictionary
        
        private AbstractPlayerState _currentState;

        #endregion
    }
}