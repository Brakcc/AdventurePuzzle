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
            var tempCc = GetComponent<CharacterController>();

            //PlayerStateDictionary
            var pSD = new Dictionary<string, AbstractPlayerState>
            {
                {"idle", new IdleState(go, ControllerState.idle)},
                {"move", new MoveState(go, ControllerState.move)},
                {"jump", new JumpState(go, ControllerState.jump)},
                {"interact", new InteractState(go, ControllerState.interact)},
                {"cancel", new CancelState(go, ControllerState.cancel)},
                {"fall", new FallState(go, ControllerState.fall)},
                {"grab", new GrabOnInterState(go, ControllerState.grab)},
                {"lever", new LockedOnLeverState(go, ControllerState.lever)},
                {"camera", new CameraFocusState(go, ControllerState.camera)},
                {"cineIdle", new CinematicIdleForcedState(go, ControllerState.cineIdle)},
                {"cineMove", new CinematicMoveForcedState(go, ControllerState.cineMove)}
            };
            
            _stateMachine.SetCallBacks((int)pSD["idle"].StateFlag, "idle", pSD["idle"].OnInit, null, 
                                       pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, null, null);
            
            _stateMachine.SetCallBacks((int)pSD["move"].StateFlag, "move", pSD["move"].OnInit, pSD["move"].OnEnterState, 
                                       pSD["move"].OnUpdate, pSD["move"].OnFixedUpdate, null, null);
            
            _stateMachine.SetCallBacks((int)pSD["jump"].StateFlag, "jump", pSD["jump"].OnInit, pSD["jump"].OnEnterState, 
                                       pSD["jump"].OnUpdate, pSD["jump"].OnFixedUpdate, null, null);
            
            _stateMachine.SetCallBacks((int)pSD["interact"].StateFlag, "interact", pSD["interact"].OnInit, pSD["interact"].OnEnterState, 
                                       pSD["interact"].OnUpdate, pSD["interact"].OnFixedUpdate, null, null);
            
            _stateMachine.SetCallBacks((int)pSD["cancel"].StateFlag, "cancel", pSD["cancel"].OnInit, pSD["cancel"].OnEnterState, 
                                       pSD["cancel"].OnUpdate, pSD["cancel"].OnFixedUpdate, null, null);
            
            _stateMachine.SetCallBacks((int)pSD["fall"].StateFlag, "fall", pSD["fall"].OnInit, null, 
                                       pSD["fall"].OnUpdate, pSD["fall"].OnFixedUpdate, pSD["fall"].OnExitState, null);
            
            _stateMachine.SetCallBacks((int)pSD["grab"].StateFlag, "grab", pSD["grab"].OnInit, pSD["grab"].OnEnterState, 
                                       pSD["grab"].OnUpdate, pSD["grab"].OnFixedUpdate, pSD["grab"].OnExitState, null);
            
            _stateMachine.SetCallBacks((int)pSD["lever"].StateFlag, "lever", pSD["lever"].OnInit, pSD["lever"].OnEnterState, 
                                       pSD["lever"].OnUpdate, pSD["lever"].OnFixedUpdate, pSD["lever"].OnExitState, null);
            
            _stateMachine.SetCallBacks((int)pSD["camera"].StateFlag, "camera", pSD["idle"].OnInit, pSD["camera"].OnEnterState, 
                                       pSD["camera"].OnUpdate, pSD["camera"].OnFixedUpdate, pSD["camera"].OnExitState, null);
            
            _stateMachine.SetCallBacks((int)pSD["cineIdle"].StateFlag, "cineIdle", pSD["cineIdle"].OnInit, pSD["cineIdle"].OnEnterState, 
                                       pSD["cineIdle"].OnUpdate, pSD["cineIdle"].OnFixedUpdate, pSD["cineIdle"].OnExitState, pSD["cineIdle"].OnCoroutine);
            
            _stateMachine.SetCallBacks((int)pSD["cineMove"].StateFlag, "cineMove", pSD["cineMove"].OnInit, pSD["cineMove"].OnEnterState, 
                                       pSD["cineMove"].OnUpdate, pSD["cineMove"].OnFixedUpdate, pSD["cineMove"].OnExitState, pSD["cineMove"].OnCoroutine);
            
            foreach (var state in pSD.Values)
            {
                state.SetCharaCont(tempCc);
                state.SetDatas(datasSo);
                state.SetChecker(checker);
                state.OnInit(_stateMachine);
            }
        }

        private void Start() => _stateMachine.StartMachine();
        
        private void Update() => _stateMachine.UpdateMachine();

        private void FixedUpdate() => _stateMachine.FixedUpdateMachine();
        
        #endregion
        
        #region fields
        
        [FieldCompletion] [SerializeField] protected BasePlayerDatasSO datasSo;

        [FieldCompletion] public InterCheckerState checker;

        private GenericStateMachine _stateMachine;

        #endregion
    }
}