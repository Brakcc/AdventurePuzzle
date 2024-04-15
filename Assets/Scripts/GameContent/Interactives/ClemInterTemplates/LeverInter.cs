using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class LeverInter : BaseInterBehavior
    {
        #region methodes

        protected override void OnInit()
        {
            base.OnInit();
            debugText = "Maintain <b>E</b> to interact";
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