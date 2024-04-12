using System;
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
                if (_currentAppliedEnergy == EnergyTypes.None)
                    OnCancelEnergy();

                else
                    InterAction();
            }
        }

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
            isActivated = false;
            _col = GetComponent<Collider>();
        }

        public override void PlayerAction()
        {
            Debug.Log($"player action {this}");
            //link this sur le player pour allow les actions
        }

        public override void PlayerCancel()
        {
            Debug.Log($"player cancel {this}");
            //delink this du player pour lâcher this
        }

        public override void InterAction() 
        {
            Debug.Log($"inter action {this}");
            //appliquer un effet selon la couleur 
            //Passer par switch case
            switch (_currentAppliedEnergy)
            {
                case EnergyTypes.None:
                    _col.enabled = true;
                    break;
                case EnergyTypes.Yellow:
                    _col.enabled = true;
                    break;
                case EnergyTypes.Green:
                    _col.enabled = false;
                    break;
                case EnergyTypes.Blue:
                    _col.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"how did that happened");
            }
        }

        private void OnCancelEnergy()
        {
            isActivated = false;
            animator.SetTrigger(0);
        }
        
        #endregion

        #region fields

        [FieldCompletion] [SerializeField] private Animator animator;

        private Collider _col;

        private EnergyTypes _currentAppliedEnergy;

        #endregion
    }
}