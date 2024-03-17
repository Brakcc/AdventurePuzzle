using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class EnergySourceInter : BaseInterBehavior
    {
        #region methodes

        protected override void OnSubscribe()
        {
            AbsorbState.OnAbsorb += Effect;
        }

        protected override void OnUnSubscribe()
        {
            AbsorbState.OnAbsorb -= Effect;
        }

        protected override void Effect()
        {
            Debug.Log("absorb");
            base.Effect();
        }

        #endregion
    }
}