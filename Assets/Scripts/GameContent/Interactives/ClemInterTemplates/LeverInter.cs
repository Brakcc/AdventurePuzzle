using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class LeverInter : BaseInterBehavior
    {
        #region methodes
        
        protected override void OnInit()
        {
            isActivated = true;
        }
        
        public override void PlayerAction()
        {
            Debug.Log($"player action {this}");
        }

        public override void InterAction()
        {
            Debug.Log($"player action {this}");
        }
        
        #endregion
    }
}