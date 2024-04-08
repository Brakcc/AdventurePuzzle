using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class EnergySourceInter : BaseInterBehavior
    {
        #region methodes

        public override void PlayerAction()
        {
            Debug.Log("player action");
        }

        public override void InterAction()
        {
            Debug.Log("inter action");
        }

        #endregion
    }
}