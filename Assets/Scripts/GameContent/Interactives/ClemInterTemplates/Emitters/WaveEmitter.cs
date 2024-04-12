using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class WaveEmitter : EmitterInter
    {
        #region methodes

        protected override void OnInit()
        {
            base.OnInit();
            if (recepDatas.Count == 0)
                return;
            
            foreach (var r in recepDatas)
            {
                r.ReceptorInter.EmitRef = this;
            }
            recepDatas.Sort(Compare);
        }

        public override void InterAction()
        {
            base.InterAction();
            recepDatas.Sort(Compare);
        }

        public override void PlayerAction()
        {
            base.PlayerAction();
        }

        public override void PlayerCancel()
        {
            base.PlayerCancel();
        }

        private IEnumerator WaveStarted()
        {
            var tempTime = 0f;
            yield break;
        }
        
        #endregion

        #region fields

        [SerializeField] private List<RecepDatas> recepDatas;

        private readonly Comparison<RecepDatas> Compare = (a, b) =>
            Mathf.RoundToInt(Mathf.Sign(a.ActivationDelay + a.ReceptorInter.DistFromEmit -
                            (b.ActivationDelay + b.ReceptorInter.DistFromEmit)));

        #endregion
    }
}