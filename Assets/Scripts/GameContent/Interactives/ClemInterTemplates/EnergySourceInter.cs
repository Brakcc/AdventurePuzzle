using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class EnergySourceInter : BaseInterBehavior
    {
        #region methodes

        public override void PlayerAction()
        {
            Debug.Log("absorb");
        }

        public override void InterAction()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}