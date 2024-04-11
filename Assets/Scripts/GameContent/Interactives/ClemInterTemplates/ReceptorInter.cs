using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.Interactives.ClemInterTemplates
{
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

        #endregion
        
        #region methodes

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
        }

        private void OnCancelEnergy()
        {
            isActivated = false;
            animator.SetTrigger(0);
        }
            
        
        #endregion

        #region fields

        [FieldCompletion] [SerializeField] private Animator animator;

        private EnergyTypes _currentAppliedEnergy;

        #endregion
    }
}