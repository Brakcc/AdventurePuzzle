using System;
using System.Collections;
using System.Collections.Generic;
using GameContent.PlayerScripts;
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
            
        }

        public override void PlayerAction()
        {
            if (SourceDatasList.Count >= 3)
                         return;
                     
            SourceDatasList.Add(PlayerEnergyM.CurrentSource);
            PlayerEnergyM.CurrentSource = new SourceDatas(null);
            PlayerEnergyM.OnSourceChangedDebug();
            
            recepDatas.Sort(Compare);
        }

        public override void PlayerCancel()
        {
            if (Count <= 0)
                return;
            
            SourceDatasList[Count - 1].Source.InterAction();
            SourceDatasList.RemoveAt(Count - 1);
            base.PlayerCancel();
        }

        private IEnumerator WaveStarted()
        {
            //var tempTime = 0f;
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