using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class ReceptorInter : BaseInterBehavior
    {
        #region methodes

        public override void PlayerAction()
        {
            Debug.Log("player action");
        }

        public override void PlayerCancel()
        {
            Debug.Log("player cancel");
        }

        public override void InterAction() 
        {
            Debug.Log("inter action");
        }

        #endregion
    }
}