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

        public Vector3 TempDir
        {
            get => _tempDir;
            set => _tempDir = SetDir(value);
        }
        
        public bool IsMovable => _isMovable;
        
        #region IsHittingTR
        
        public bool IsHittingTopRight => Physics.Linecast(
            _col.bounds.center + new Vector3(_col.bounds.extents.x + widthCorrector, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z), 
            _col.bounds.center + new Vector3(_col.bounds.extents.x + widthCorrector, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -_col.bounds.extents.z),
            blockMask) || 
                                         Physics.Linecast(
            _col.bounds.center + new Vector3(_col.bounds.extents.x + widthCorrector, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -_col.bounds.extents.z), 
            _col.bounds.center + new Vector3(_col.bounds.extents.x + widthCorrector, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z), 
            blockMask);
        
        #endregion
        
        #region IsHittingTL
        
        public bool IsHittingTopLeft => Physics.Linecast(
            _col.bounds.center + new Vector3(_col.bounds.extents.x, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z + widthCorrector), 
            _col.bounds.center + new Vector3(-_col.bounds.extents.x, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z + widthCorrector),
            blockMask) || 
                                        Physics.Linecast(
            _col.bounds.center + new Vector3(-_col.bounds.extents.x,
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z + widthCorrector), 
            _col.bounds.center + new Vector3(_col.bounds.extents.x, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z + widthCorrector), 
            blockMask);
        
        #endregion
        
        #region IsHittingBR
        
        public bool IsHittingBottomRight => Physics.Linecast(
            _col.bounds.center + new Vector3(-_col.bounds.extents.x, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -(_col.bounds.extents.z + widthCorrector)), 
            _col.bounds.center + new Vector3(_col.bounds.extents.x, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -(_col.bounds.extents.z + widthCorrector)),
            blockMask) ||
                                            Physics.Linecast(
            _col.bounds.center + new Vector3(_col.bounds.extents.x, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -(_col.bounds.extents.z + widthCorrector)), 
            _col.bounds.center + new Vector3(-_col.bounds.extents.x, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -(_col.bounds.extents.z + widthCorrector)),
            blockMask);
        
        #endregion
        
        #region IsHittingBL
        
        public bool IsHittingBottomLeft => Physics.Linecast(
            _col.bounds.center + new Vector3(-(_col.bounds.extents.x + widthCorrector), 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z), 
            _col.bounds.center + new Vector3(-(_col.bounds.extents.x + widthCorrector), 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -_col.bounds.extents.z),
            blockMask) || 
                                           Physics.Linecast(
            _col.bounds.center + new Vector3(-(_col.bounds.extents.x + widthCorrector), 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -_col.bounds.extents.z), 
            _col.bounds.center + new Vector3(-(_col.bounds.extents.x + widthCorrector), 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z),
            blockMask);

        #endregion

        #region HasBlueAbove

        public List<ReceptorInter> TopReceps => _grabber.RecepRefs;

        #endregion
        
        #endregion

        #region methodes

        protected override void OnInit()
        {
            hasElectricity = false;
            _col = GetComponent<Collider>();
            _rb = GetComponent<Rigidbody>();
            _grabber = GetComponentInChildren<RecepsTopBlockGrabber>();

            _rb.mass = 1000;
            _rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
            _rb.isKinematic = true;

            _tempDir = Vector3.zero;

            if (debugMod.hasLight)
            {
                InterLight = debugMod.debugLight;
                OnChangeColorLightDebug(CurrentEnergyType);
            }
            
            OnReset();
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
                    _rb.isKinematic = true;
                    _rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
                    debugTextLocal = "";
                    break;
                case EnergyTypes.Yellow:
                    _col.enabled = true;
                    hasElectricity = true;
                    _isMovable = false;
                    _rb.isKinematic = true;
                    _rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
                    debugTextLocal = "";
                    break;
                case EnergyTypes.Green:
                    _col.enabled = false;
                    hasElectricity = false;
                    _isMovable = false;
                    _rb.isKinematic = true;
                    _rb.constraints = GetRBConstraints(RBCMode.Full);
                    debugTextLocal = "";
                    break;
                case EnergyTypes.Blue:
                    _col.enabled = true;
                    hasElectricity = false;
                    _isMovable = true;
                    _rb.isKinematic = false;
                    _rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
                    debugTextLocal = debugMod.debugString;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_currentAppliedEnergy), _currentAppliedEnergy,
                        "how did that happened wtf ???");
            }
        }
        
        #endregion

        private Vector3 SetDir(Vector3 dir) => (dir.x, dir.z) switch
        {
            (>= Constants.MinBlockMoveInputThreshold, <= Constants.MinBlockMoveInputThreshold) => IsHittingTopRight ? Vector3.zero : dir,
            (<= Constants.MinBlockMoveInputThreshold, >= Constants.MinBlockMoveInputThreshold) => IsHittingTopLeft ? Vector3.zero : dir,
            (<= Constants.MinBlockMoveInputThreshold, <= -Constants.MinBlockMoveInputThreshold) => IsHittingBottomRight ? Vector3.zero : dir,
            (<= -Constants.MinBlockMoveInputThreshold, <= Constants.MinBlockMoveInputThreshold) => IsHittingBottomLeft ? Vector3.zero : dir,
            _ => Vector3.zero
        };

        public void MoveSolid(Vector3 dir) => _rb.position += dir;
            
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
        
        #region Gizmos
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var bounds = _col.bounds;
            
            //Top Right
            Gizmos.DrawLine(bounds.center + new Vector3(bounds.extents.x + widthCorrector, 
                                                        heightCorrector * bounds.extents.y,
                                                        bounds.extents.z), 
                             bounds.center + new Vector3(bounds.extents.x + widthCorrector, 
                                                         heightCorrector * bounds.extents.y, 
                                                         -bounds.extents.z));
            
            //Bottom Left
            Gizmos.DrawLine(bounds.center + new Vector3(-(bounds.extents.x + widthCorrector), 
                                                        heightCorrector * bounds.extents.y,
                                                        bounds.extents.z),
                            bounds.center + new Vector3(-(bounds.extents.x + widthCorrector),
                                                        heightCorrector * bounds.extents.y,
                                                        -bounds.extents.z));
            
            //Top Left
            Gizmos.DrawLine(bounds.center + new Vector3(bounds.extents.x, 
                                                        heightCorrector * bounds.extents.y, 
                                                        bounds.extents.z + widthCorrector), 
                            bounds.center + new Vector3(-bounds.extents.x, 
                                                        heightCorrector * bounds.extents.y,
                                                        bounds.extents.z + widthCorrector));
            
            //Bottom Right
            Gizmos.DrawLine(bounds.center + new Vector3(-bounds.extents.x, 
                                                        heightCorrector * bounds.extents.y,
                                                        -(bounds.extents.z + widthCorrector)), 
                            bounds.center + new Vector3(bounds.extents.x,
                                                        heightCorrector * bounds.extents.y, 
                                                        -(bounds.extents.z + widthCorrector)));
        }

        #endregion
        
        #endregion

        #region fields

        [FieldCompletion] [SerializeField] private Animator animator;

        [FieldCompletion] public Transform pivot;

        [Range(0.01f, 0.2f)] [SerializeField] private float widthCorrector;
        [Range(-1, 1)] [SerializeField] private float heightCorrector;

        [SerializeField] private LayerMask blockMask;
        
        [FieldCompletion(FieldColor.Orange)]
        [SerializeField] private Collider _col;

        private Rigidbody _rb;

        private RecepsTopBlockGrabber _grabber;

        private EnergyTypes _currentAppliedEnergy;

        private Vector3 _tempDir;

        private bool _isMovable;

        protected bool hasElectricity;

        #endregion
    }
}