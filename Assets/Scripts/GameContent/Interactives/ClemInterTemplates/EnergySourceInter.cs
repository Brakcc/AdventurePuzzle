using GameContent.PlayerScripts;
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

        public override void PlayerAction()
        {
            if (!isActivated)
                return;
            
            isActivated = false;
            OnActionAnim("isActive", isActivated);

            if (PlayerEnergyM.EnergyType != EnergyTypes.None)
            {
                PlayerEnergyM.CurrentSource.Source.InterAction();
                PlayerEnergyM.CurrentSource = new SourceDatas(this);
                PlayerEnergyM.OnSourceChangedDebug();
                return;
            }
            PlayerEnergyM.CurrentSource = new SourceDatas(this);
            PlayerEnergyM.OnSourceChangedDebug();
        }

        public override void InterAction()
        {
            if (isActivated)
                return;
            
            //mettre des lien renderer ou vfx pour montrer la libération de l'energie ?
            isActivated = true;
            OnActionAnim("isActive", isActivated);
        }

        public void OnForceAbsorb()
        {
            if (!isActivated)
                return;

            isActivated = false;
            OnActionAnim("isActive", isActivated);
        }
        
        #region anims et VFX
        
        private void OnActionAnim(string arg, bool state)
        {
            animator.SetBool(arg, state);
        }
        
        private void OnActionAnim(int id, bool state)
        {
            animator.SetBool(id, state);
        }
        
        #endregion

        #endregion

        #region fields

        [SerializeField] private EnergyTypes baseType;

        [FieldCompletion] [SerializeField] private Animator animator; 

        #endregion
    }
}