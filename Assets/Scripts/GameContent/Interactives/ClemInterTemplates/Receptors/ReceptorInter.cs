using System;
using System.Collections.Generic;
using DebuggingClem;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates.Receptors
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class ReceptorInter : BaseInterBehavior
    {
        #region properties

        #region Inner Infos
        
        public EnergyTypes CurrentEnergyType
        {
            get => _currentAppliedEnergy;
            set
            {
                _currentAppliedEnergy = value;
                InterAction();
                if (hasDebugMod)
                    OnChangeColorLightDebug(_currentAppliedEnergy);
            }
        }
        
        private Light InterLight { get; set; }

        public EmitterInter EmitRef { get; set; }

        public float DistFromEmit
        {
            get
            {
                if (EmitRef is null)
                    return 0;

                return Vector3.Distance(EmitRef.transform.position, transform.position);
            }
        }

        public Vector3 Pivot => pivot.position;
        
        public Vector3 TempDir
        {
            get => _tempDir;
            set => _tempDir = SetDir(value);
        }
        
        public bool IsMovable
        {
            get => _isMovable;
            protected set => _isMovable = value;
        }

        public bool HasWaveEnergy
        {
            get => _hasWaveEnergy;
            set
            {
                _hasWaveEnergy = value;
                _waveEnergyCounter = _hasWaveEnergy ? Constants.WaveEnergyDuration : -1;
            }
            
        }

        public bool HasCableEnergy
        {
            get => _hasCableEnergy;
            set
            {
                _hasCableEnergy = value;
            }
        }
        
        #endregion
        
        #region IsHittingTR
        
        public bool IsHittingTopRight => Physics.BoxCast(_col.bounds.center + new Vector3(
                                                          _col.bounds.extents.x + Constants.BoxCastBounds.SideBoxPosDeport, 
                                                          0, 
                                                          0),
                                                         new Vector3(
                                                                     Constants.BoxCastBounds.SideBoxHalfExtent / 2, 
                                                                     _col.bounds.extents.y - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                     _col.bounds.extents.z - Constants.BoxCastBounds.SideBoxLengthCut),
                                                         Vector3.right,
                                                         Quaternion.identity, 
                                                         Constants.BoxCastBounds.SideCastDist, 
                                                         blockMask);
        
        #endregion
        
        #region IsHittingTL
        
        public bool IsHittingTopLeft => Physics.BoxCast(_col.bounds.center + new Vector3(
                                                         0, 
                                                         0, 
                                                         _col.bounds.extents.z + Constants.BoxCastBounds.SideBoxPosDeport),
                                                        new Vector3(
                                                                    _col.bounds.extents.x - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                    _col.bounds.extents.y - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                    Constants.BoxCastBounds.SideBoxHalfExtent / 2),
                                                        Vector3.forward, 
                                                        Quaternion.identity, 
                                                        Constants.BoxCastBounds.SideCastDist, 
                                                        blockMask);
        
        #endregion
        
        #region IsHittingBR
        
        public bool IsHittingBottomRight => Physics.BoxCast(_col.bounds.center + new Vector3(
                                                             0, 
                                                             0, 
                                                             -(_col.bounds.extents.z + Constants.BoxCastBounds.SideBoxPosDeport)),
                                                            new Vector3(
                                                                        _col.bounds.extents.x - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                        _col.bounds.extents.y - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                        Constants.BoxCastBounds.SideBoxHalfExtent / 2),
                                                            Vector3.back, 
                                                            Quaternion.identity, 
                                                            Constants.BoxCastBounds.SideCastDist, 
                                                            blockMask);
        
        #endregion
        
        #region IsHittingBL
        
        public bool IsHittingBottomLeft => Physics.BoxCast(_col.bounds.center + new Vector3(
                                                            -(_col.bounds.extents.x + Constants.BoxCastBounds.SideBoxPosDeport), 
                                                            0, 
                                                            0),
                                                           new Vector3(
                                                                       Constants.BoxCastBounds.SideBoxHalfExtent / 2, 
                                                                       _col.bounds.extents.y - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                       _col.bounds.extents.z - Constants.BoxCastBounds.SideBoxLengthCut),
                                                           Vector3.left, 
                                                           Quaternion.identity, 
                                                           Constants.BoxCastBounds.SideCastDist, 
                                                           blockMask);

        #endregion

        #region IsHittingGround

        //raycast ou boxcast ?
        public bool IsHittingGround => Physics.BoxCast(_col.bounds.center + new Vector3(
                                                            0, 
                                                            -(_col.bounds.extents.y - Constants.BoxCastBounds.DownBoxPosDeport), 
                                                            0),
                                                           new Vector3(
                                                                       _col.bounds.extents.x, 
                                                                       Constants.BoxCastBounds.DownBoxHalfExtent / 2, 
                                                                       _col.bounds.extents.z),
                                                           Vector3.down, 
                                                           Quaternion.identity, 
                                                           Constants.BoxCastBounds.DownCastDist, 
                                                           blockMask);

        #endregion
        
        #region HasBlueAbove

        public List<ReceptorInter> TopReceps => _grabber.RecepRefs;
        
        public bool IsOnTop { get; set; }

        #endregion
        
        #endregion

        #region methodes

        protected override void OnInit()
        {
            hasElectricity = false;
            _col = GetComponent<Collider>();
            _rb = GetComponent<Rigidbody>();
            _grabber = GetComponentInChildren<RecepsTopBlockGrabber>();

            //_rb.mass = 1000;
            //_rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
            _rb.isKinematic = true;

            _tempDir = Vector3.zero;
            IsOnTop = false;
            _fallCurveCounter = 0;

            if (debugMod.hasLight)
            {
                InterLight = debugMod.debugLight;
                OnChangeColorLightDebug(CurrentEnergyType);
            }
            
            OnReset();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (!HasWaveEnergy)
                return;

            if (_waveEnergyCounter <= 0)
            {
                HasWaveEnergy = false;
                CurrentEnergyType = EnergyTypes.None;
                return;
            }

            _waveEnergyCounter -= Time.deltaTime;
        }

        protected override void OnFixedUpdate()
        {
            SolidFall();
        }

        #region Actions

        public override void PlayerAction()
        {
            //Debug.Log($"player action {this}");
            //link this sur le player pour allow les actions
        }

        public override void PlayerCancel()
        {
            //Debug.Log($"player cancel {this}");
            //delink this du player pour lacher this
        }

        public override void InterAction()
        {
            //Debug.Log($"inter action {this}");
            switch (CurrentEnergyType)
            {
                case EnergyTypes.None:
                    _col.enabled = true;
                    hasElectricity = false;
                    _isMovable = false;
                    //_rb.isKinematic = true;
                    //_rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
                    debugTextLocal = "";
                    break;
                case EnergyTypes.Yellow:
                    _col.enabled = true;
                    hasElectricity = true;
                    _isMovable = false;
                    //_rb.isKinematic = true;
                    //_rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
                    debugTextLocal = "";
                    break;
                case EnergyTypes.Green:
                    _col.enabled = false;
                    hasElectricity = false;
                    _isMovable = false;
                    //_rb.isKinematic = true;
                    //_rb.constraints = GetRBConstraints(RBCMode.Full);
                    debugTextLocal = "";
                    break;
                case EnergyTypes.Blue:
                    _col.enabled = true;
                    hasElectricity = false;
                    _isMovable = true;
                    //_rb.isKinematic = true;
                    //_rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
                    debugTextLocal = debugMod.debugString;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_currentAppliedEnergy), _currentAppliedEnergy,
                        "how did that happened wtf ???");
            }
        }
        
        #endregion

        #region Inner Actions
        
        private Vector3 SetDir(Vector3 dir) => (dir.x, dir.z) switch
        {
            (>= Constants.MinBlockMoveInputThreshold, <= Constants.MinBlockMoveInputThreshold) => IsHittingTopRight ? Vector3.zero : dir,
            (<= Constants.MinBlockMoveInputThreshold, >= Constants.MinBlockMoveInputThreshold) => IsHittingTopLeft ? Vector3.zero : dir,
            (<= Constants.MinBlockMoveInputThreshold, <= -Constants.MinBlockMoveInputThreshold) => IsHittingBottomRight ? Vector3.zero : dir,
            (<= -Constants.MinBlockMoveInputThreshold, <= Constants.MinBlockMoveInputThreshold) => IsHittingBottomLeft ? Vector3.zero : dir,
            _ => Vector3.zero
        };

        private void SolidFall()
        {
            if (IsHittingGround)
            {
                if (_fallCurveCounter > 0)
                    _fallCurveCounter = 0;
                return;
            }

            _fallCurveCounter += Time.fixedDeltaTime;
            transform.position -= new Vector3(0, fallCurve.Evaluate(_fallCurveCounter) * Time.fixedDeltaTime, 0);
        }
        
        public void MoveSolid(Vector3 dir) => _rb.transform.position += dir;
            
        public void SetRBConstraints(RigidbodyConstraints constraints) => _rb.constraints = constraints;
        
        public virtual void OnReset()
        {
            CurrentEnergyType = EnergyTypes.None;
        }

        private void OnChangeColorLightDebug(EnergyTypes type) => InterLight.color = LightDebugger.DebugColor(type);

        public static RigidbodyConstraints GetRBConstraints(RBCMode mode) => mode switch
        {
            RBCMode.Rota => RigidbodyConstraints.FreezeRotation,
            RBCMode.RotaPlan => (RigidbodyConstraints)Constants.BitFlagRBConstraintRotaPlan,
            RBCMode.Full => RigidbodyConstraints.FreezeAll,
            RBCMode.None => RigidbodyConstraints.None,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "nice try my guy")
        };
        
        #endregion
        
        #region Gizmos
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var bounds = _col.bounds;
            
            #region TR
            
            //Top Right
            Gizmos.DrawWireCube(bounds.center + new Vector3(bounds.extents.x + 0.1f, 0, 0),
                                new Vector3(0.15f, bounds.size.y - 0.2f, bounds.size.z - 0.2f));
            
            #endregion
            
            #region BL
            
            //Bottom Left
            Gizmos.DrawWireCube(bounds.center + new Vector3(-(bounds.extents.x + 0.1f), 0, 0),
                                new Vector3(0.15f, bounds.size.y - 0.2f, bounds.size.z - 0.2f));
            
            #endregion
            
            #region TL
            
            //Top Left
            Gizmos.DrawWireCube(bounds.center + new Vector3(0, 0, bounds.extents.z + 0.1f),
                                new Vector3(bounds.size.x - 0.2f, bounds.size.y - 0.2f, 0.15f));
            
            #endregion
            
            #region BR
            
            //Bottom Right
            Gizmos.DrawWireCube(bounds.center + new Vector3(0, 0, -(bounds.extents.z + 0.1f)),
                                new Vector3(bounds.size.x - 0.2f, bounds.size.y - 0.2f, 0.15f));
            #endregion

            #region GC

            Gizmos.DrawWireCube(bounds.center + new Vector3(0, -(bounds.extents.y/* - 0.03f*/), 0), 
                new Vector3(bounds.size.x, 0.15f, bounds.size.z));

            #endregion
        }

        #endregion
        
        #endregion

        #region fields

        [FieldCompletion] [SerializeField] private Animator animator;

        [FieldCompletion] [SerializeField] private Transform pivot;

        [SerializeField] private LayerMask blockMask;

        [SerializeField] private AnimationCurve fallCurve;
        
        [FieldCompletion(FieldColor.Orange)]
        [SerializeField] private Collider _col;

        private Rigidbody _rb;

        private RecepsTopBlockGrabber _grabber;

        private EnergyTypes _currentAppliedEnergy;

        private Vector3 _tempDir;

        private bool _isMovable;

        private bool _hasWaveEnergy;

        private bool _hasCableEnergy;

        private float _waveEnergyCounter;
        
        protected bool hasElectricity;

        private float _fallCurveCounter;

        #endregion
    }
}