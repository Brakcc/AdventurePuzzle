using System;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class WaveEmitter : EmitterInter
    {
        #region methodes

        public override void InterAction()
        {
            base.InterAction();
        }

        public override void PlayerAction()
        {
            base.PlayerAction();
        }

        public override void PlayerCancel()
        {
            base.PlayerCancel();
        }

        #endregion

        #region fields

        private readonly Comparison<int> Compare = (a, b) => (int)Mathf.Sign(b - a);

        #endregion
    }
}