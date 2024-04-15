using System;
using DebuggingClem;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.Interactives.ClemInterTemplates
{
    [RequireComponent(typeof(Collider))]
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
                OnChangeColorLightDebug(_currentAppliedEnergy);
            }
        }
        
        private Light InterLight { get; set; }
        
        public EmitterInter EmitRef { get; set; }

        public float DistFromEmit
        {
            get
            {
                if (EmitRef == null)
                    return 0;
                
                return Vector3.Distance(EmitRef.transform.position, transform.position);
            }
        }

        #endregion
        
        #region methodes

        protected override void OnInit()
        {
            hasElectricity = false;
            _col = GetComponent<Collider>();
            InterLight = GetComponentInChildren<Light>();
            OnChangeColorLightDebug(CurrentEnergyType);
        }

        public override void PlayerAction()
        {
            //Debug.Log($"player action {this}");
            //link this sur le player pour allow les actions
        }

        public override void PlayerCancel()
        {
            //Debug.Log($"player cancel {this}");
            //delink this du player pour lâcher this
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
                    debugText = "";
                    break;
                case EnergyTypes.Yellow:
                    _col.enabled = true;
                    hasElectricity = true;
                    isMovable = false;
                    debugText = "";
                    break;
                case EnergyTypes.Green:
                    _col.enabled = false;
                    hasElectricity = false;
                    isMovable = false;
                    debugText = "";
                    break;
                case EnergyTypes.Blue:
                    _col.enabled = true;
                    hasElectricity = false;
                    isMovable = true;
                    debugText = "Maintain <b>E</b> to interact";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_currentAppliedEnergy), _currentAppliedEnergy,"how did that happened wtf ???");
            }
        }

        public void OnReset()
        {
            CurrentEnergyType = EnergyTypes.None;
            _col.enabled = true;
            hasElectricity = false;
            isMovable = false;
        }
        
        private void OnChangeColorLightDebug(EnergyTypes type) => InterLight.color = LightDebugger.DebugColor(type);
        
        #endregion

        #region fields

        [FieldCompletion] [SerializeField] private Animator animator;

        private Collider _col;

        private EnergyTypes _currentAppliedEnergy;

        protected bool hasElectricity;

        protected bool isMovable;

        #endregion
    }
}