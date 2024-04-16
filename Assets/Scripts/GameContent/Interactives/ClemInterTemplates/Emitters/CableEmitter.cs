using GameContent.PlayerScripts;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class CableEmitter : EmitterInter
    {
        #region methodes

        protected override void OnInit()
        {
            base.OnInit();
            if (receptors.Length == 0)
                return;
            
            foreach (var r in receptors)
            {
                r.EmitRef = this;
            }
        }
        
        public override void InterAction()
        {
            for (var i = 0; i < Count; i++)
            {
                receptors[i].CurrentEnergyType = this[i].Type;
            }
        }

        public override void PlayerAction()
        {
            if (SourceDatasList.Count >= 3)
                return;
            
            SourceDatasList.Add(PlayerEnergyM.CurrentSource);
            PlayerEnergyM.CurrentSource = new SourceDatas(null);
            PlayerEnergyM.OnSourceChangedDebug();
            
            InterAction();
        }

        public override void PlayerCancel()
        {
            if (Count <= 0)
                return;
            
            SourceDatasList[Count - 1].Source.InterAction();
            receptors[Count - 1].OnReset();
            SourceDatasList.RemoveAt(Count - 1);
            base.PlayerCancel();
        }

        #endregion

        #region fields
        
        [FieldCompletion(FieldColor.Yellow, FieldColor.Green)] 
        [SerializeField] private ReceptorInter[] receptors;

        #endregion
    }
}