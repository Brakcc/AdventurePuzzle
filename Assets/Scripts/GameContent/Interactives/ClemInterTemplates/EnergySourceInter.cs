using GameContent.PlayerScripts;
using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.Interactives.ClemInterTemplates
{
    public sealed class EnergySourceInter : BaseInterBehavior
    {
        #region properties

        public EnergyTypes EnergyType => baseType;

        #endregion
        
        #region methodes

        protected override void OnInit()
        {
            isActivated = true;
        }

        public override void PlayerAction()
        {
            Debug.Log($"player action {this}");
            if (!isActivated)
                return;
            
            isActivated = !isActivated;
            //OnActionAnim(0, isActivated);

            if (PlayerEnergyM.EnergyType == EnergyTypes.None)
                return;
            
            PlayerEnergyM.CurrentSource.Source.InterAction();
            PlayerEnergyM.CurrentSource = new SourceDatas(this);
        }

        public override void InterAction()
        {
            Debug.Log($"inter action {this}");
            if (isActivated)
                return;
            
            //mettre des lien renderer ou vfx pour montrer la libération de l'energie ?
            isActivated = !isActivated;
            //OnActionAnim(0, isActivated);
        }

        private void OnActionAnim(string arg, bool state)
        {
            animator.SetBool(arg, state);
        }
        
        private void OnActionAnim(int id, bool state)
        {
            animator.SetBool(id, state);
        }

        #endregion

        #region fields

        [SerializeField] private EnergyTypes baseType;

        [FieldCompletion] [SerializeField] private Animator animator; 

        #endregion
    }
}