using System.Collections;
using System.Collections.Generic;
using GameContent.CameraScripts;
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

        public BasePlayerDatasSO DatasSo => datasSo;

        public Animator PlayerAnimator => animator;

        public InterCheckerState CheckerState => checker;

        internal CameraDatas CurrentCameraDatas { get; set; }

        public CameraDatas TransitionCamDatas => transCamDatas;
        
        public CameraDatas InitCamDatas => initCamDatas;

        public CameraManager InitCamManager => initCamManager;

        public CameraManager TransCamManager => transCamManager;

        private Vector3 LastPos
        {
            get
            {
                Physics.Raycast(transform.position, Vector3.down, out var hit, 2,
                    datasSo.groundingDatasSo.groundLayer);

                return hit.point + new Vector3(0, CharaCont.height / 2 + 0.5f, 0);
            }
        }
        
        internal bool IsDedge { get; set; }
        
        internal float CamLerpCoef { get; set; }
        
        internal float MoveAnimLerpCoef { get; set; }
        
        public static int InitCamAngle => InitialCameraAngle;
        
        public CharacterController CharaCont => GetComponent<CharacterController>();

        #endregion
        
        #region methodes

        #region GenericStateMachine
        
        private void Awake()
        {
            AnimationManager.InitAnimationManager(animator);
            
            _stateMachine = new GenericStateMachine(11);
            var go = gameObject;

            //PlayerStateDictionary
            var pSD = new Dictionary<string, AbstractGenericState>
            {
                {"idle", new IdleState(go, ControllerState.idle, this)},
                {"move", new MoveState(go, ControllerState.move, this)},
                {"jump", new JumpState(go, ControllerState.jump, this)},
                {"interact", new InteractState(go, ControllerState.interact, this)},
                {"cancel", new CancelState(go, ControllerState.cancel, this)},
                {"fall", new FallState(go, ControllerState.fall, this)},
                {"grab", new GrabOnInterState(go, ControllerState.grab, this)},
                {"lever", new LockedOnLeverState(go, ControllerState.lever, this)},
                {"camera", new CameraFocusState(go, ControllerState.camera, this)},
                {"cineIdle", new CinematicIdleForcedState(go, ControllerState.cineIdle, this)},
                {"cineMove", new CinematicMoveForcedState(go, ControllerState.cineMove, this)}
            };
            
            _stateMachine.SetCallBacks((byte)ControllerState.idle, "idle", pSD["idle"].OnInit, pSD["idle"].OnEnterState, 
                                       pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.move, "move", pSD["move"].OnInit, pSD["move"].OnEnterState, 
                                       pSD["move"].OnUpdate, pSD["move"].OnFixedUpdate, pSD["move"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.jump, "jump", pSD["jump"].OnInit, pSD["jump"].OnEnterState, 
                                       pSD["jump"].OnUpdate, pSD["jump"].OnFixedUpdate, null, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.interact, "interact", pSD["interact"].OnInit, pSD["interact"].OnEnterState, 
                                       pSD["interact"].OnUpdate, pSD["interact"].OnFixedUpdate, pSD["interact"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.cancel, "cancel", pSD["cancel"].OnInit, pSD["cancel"].OnEnterState, 
                                       pSD["cancel"].OnUpdate, pSD["cancel"].OnFixedUpdate, null, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.fall, "fall", pSD["fall"].OnInit, pSD["fall"].OnEnterState, 
                                       pSD["fall"].OnUpdate, pSD["fall"].OnFixedUpdate, pSD["fall"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.grab, "grab", pSD["grab"].OnInit, pSD["grab"].OnEnterState, 
                                       pSD["grab"].OnUpdate, pSD["grab"].OnFixedUpdate, pSD["grab"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.lever, "lever", pSD["lever"].OnInit, pSD["lever"].OnEnterState, 
                                       pSD["lever"].OnUpdate, pSD["lever"].OnFixedUpdate, pSD["lever"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.camera, "camera", pSD["camera"].OnInit, pSD["camera"].OnEnterState, 
                                       pSD["camera"].OnUpdate, pSD["camera"].OnFixedUpdate, pSD["camera"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.cineIdle, "cineIdle", pSD["cineIdle"].OnInit, pSD["cineIdle"].OnEnterState, 
                                       pSD["cineIdle"].OnUpdate, pSD["cineIdle"].OnFixedUpdate, pSD["cineIdle"].OnExitState, pSD["cineIdle"].OnCoroutine);
            
            _stateMachine.SetCallBacks((byte)ControllerState.cineMove, "cineMove", pSD["cineMove"].OnInit, pSD["cineMove"].OnEnterState, 
                                       pSD["cineMove"].OnUpdate, pSD["cineMove"].OnFixedUpdate, pSD["cineMove"].OnExitState, pSD["cineMove"].OnCoroutine);

            _stateMachine.InitMachine();
        }

        private void Start() => _stateMachine.StartMachine();
        
        private void Update()
        {
            if (!IsDedge)
            {
                _stateMachine.UpdateMachine();
                _lastPos = LastPos;
                return;
            }

            _dedTimeCounter += Time.deltaTime;

            if (_dedTimeCounter > 0.35f)
                transform.position = _lastPos;

            if (_dedTimeCounter <= 0.55f)
                return;
            
            _dedTimeCounter = 0;
            IsDedge = false;
        }

        private void FixedUpdate()
        {
            if (!IsDedge)
                _stateMachine.FixedUpdateMachine();
        }
        
        #endregion
        
        #endregion
        
        #region fields
        
        [FieldCompletion] [SerializeField] private BasePlayerDatasSO datasSo;

        [FieldCompletion] [SerializeField] private Animator animator;
        
        [FieldCompletion] [SerializeField] private InterCheckerState checker;

        [FieldCompletion] [SerializeField] private CameraManager initCamManager;

        [FieldCompletion] [SerializeField] private CameraManager transCamManager;
        
        [SerializeField] private CameraDatas initCamDatas;
        
        [SerializeField] private CameraDatas transCamDatas;
        
        private GenericStateMachine _stateMachine;

        private Vector3 _lastPos;
        
        private float _dedTimeCounter;
        
        private const int InitialCameraAngle = 45;

        #endregion
    }
}