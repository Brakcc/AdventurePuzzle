using System;
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
        
        public bool IsHittingTopRight => Physics.Linecast(
            _col.bounds.center + new Vector3(_col.bounds.extents.x + widthCorrector, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z + widthCorrector), 
            _col.bounds.center + new Vector3(_col.bounds.extents.x + widthCorrector, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -(_col.bounds.extents.z + widthCorrector)),
            blockMask);
        
        public bool IsHittingTopLeft => Physics.Linecast(
            _col.bounds.center + new Vector3(_col.bounds.extents.x + widthCorrector, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z + widthCorrector), 
            _col.bounds.center + new Vector3(-(_col.bounds.extents.x + widthCorrector), 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z + widthCorrector),
            blockMask);
        
        public bool IsHittingBottomRight => Physics.Linecast(
            _col.bounds.center + new Vector3(-(_col.bounds.extents.x + widthCorrector), 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -(_col.bounds.extents.z + widthCorrector)), 
            _col.bounds.center + new Vector3(_col.bounds.extents.x + widthCorrector, 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -(_col.bounds.extents.z + widthCorrector)),
            blockMask);
        
        public bool IsHittingBottomLeft => Physics.Linecast(
            _col.bounds.center + new Vector3(-(_col.bounds.extents.x + widthCorrector), 
                                             heightCorrector * _col.bounds.extents.y, 
                                             _col.bounds.extents.z + widthCorrector), 
            _col.bounds.center + new Vector3(-(_col.bounds.extents.x + widthCorrector), 
                                             heightCorrector * _col.bounds.extents.y, 
                                             -(_col.bounds.extents.z + widthCorrector)),
            blockMask);

        #endregion

        #region methodes

        protected override void OnInit()
        {
            hasElectricity = false;
            _col = GetComponent<Collider>();
            _rb = GetComponent<Rigidbody>();

            _rb.mass = 1000;
            _rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
            _rb.isKinematic = true;

            if (debugMod.hasLight)
            {
                InterLight = debugMod.debugLight;
                OnChangeColorLightDebug(CurrentEnergyType);
            }

            OnReset();
        }

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
                    isMovable = false;
                    _rb.isKinematic = true;
                    _rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
                    debugTextLocal = "";
                    break;
                case EnergyTypes.Yellow:
                    _col.enabled = true;
                    hasElectricity = true;
                    isMovable = false;
                    _rb.isKinematic = true;
                    _rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
                    debugTextLocal = "";
                    break;
                case EnergyTypes.Green:
                    _col.enabled = false;
                    hasElectricity = false;
                    isMovable = false;
                    _rb.isKinematic = true;
                    _rb.constraints = GetRBConstraints(RBCMode.Full);
                    debugTextLocal = "";
                    break;
                case EnergyTypes.Blue:
                    _col.enabled = true;
                    hasElectricity = false;
                    isMovable = true;
                    _rb.isKinematic = false;
                    _rb.constraints = GetRBConstraints(RBCMode.RotaPlan);
                    debugTextLocal = debugMod.debugString;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_currentAppliedEnergy), _currentAppliedEnergy,
                        "how did that happened wtf ???");
            }
        }

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
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "nice try my guy ???")
        };
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var bounds = _col.bounds;
            
            Gizmos.DrawLine(bounds.center + new Vector3(bounds.extents.x + widthCorrector, 
                                                              heightCorrector * bounds.extents.y, 
                                                              bounds.extents.z + widthCorrector), 
                             bounds.center + new Vector3(bounds.extents.x + widthCorrector, 
                                                              heightCorrector * bounds.extents.y, 
                                                              -(bounds.extents.z + widthCorrector)));
            
            Gizmos.DrawLine(_col.bounds.center + new Vector3(-(bounds.extents.x + widthCorrector), 
                                                             heightCorrector * bounds.extents.y, 
                                                             bounds.extents.z + widthCorrector), 
                            bounds.center + new Vector3(-(bounds.extents.x + widthCorrector),
                                                             heightCorrector * bounds.extents.y,
                                                             -(bounds.extents.z + widthCorrector)));
            
            Gizmos.DrawLine(_col.bounds.center + new Vector3(bounds.extents.x + widthCorrector, 
                                                             heightCorrector * bounds.extents.y, 
                                                             bounds.extents.z + widthCorrector), 
                            bounds.center + new Vector3(-(bounds.extents.x + widthCorrector), 
                                                             heightCorrector * bounds.extents.y,
                                                             bounds.extents.z + widthCorrector));
            
            Gizmos.DrawLine(_col.bounds.center + new Vector3(-(bounds.extents.x + widthCorrector), 
                                                             heightCorrector * bounds.extents.y,
                                                             -(bounds.extents.z + widthCorrector)), 
                            bounds.center + new Vector3(bounds.extents.x + widthCorrector,
                                                             heightCorrector * bounds.extents.y, 
                                                             -(bounds.extents.z + widthCorrector)));
        }

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

        private EnergyTypes _currentAppliedEnergy;

        protected bool hasElectricity;

        protected bool isMovable;

        #endregion
    }
}