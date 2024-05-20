using GameContent.Interactives.ClemInterTemplates.Receptors;
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
            if (nodes.Length == 0)
                return;
            
            foreach (var n in nodes)
            {
                if (n.dendrite is DentriteType.Receptor)
                    n.receptorRef.EmitRef = this;
            }
        }
        
        public override void InterAction()
        {
            //ici action du cable
            //
            //
            //
            
            for (var i = 0; i < SourceCount; i++)
            {
                if (nodes[i].dendrite is DentriteType.Receptor)
                    nodes[i].receptorRef.CurrentEnergyType = this[i].Type;
            }
        }

        public override void PlayerAction()
        {
            if (SourceDatasList.Count >= nodes.Length)
                return;
            
            if (PlayerEnergyM.EnergyType == EnergyTypes.None)
                return;
            
            SourceDatasList.Add(PlayerEnergyM.CurrentSource);
            PlayerEnergyM.CurrentSource = new SourceDatas();
            PlayerEnergyM.OnSourceChangedDebug();
            
            InterAction();
        }

        public override void PlayerCancel()
        {
            if (SourceCount <= 0)
                return;

            if (PlayerEnergyM.GetEnergyBack)
            {
                if (PlayerEnergyM.EnergyType != EnergyTypes.None)
                    PlayerEnergyM.CurrentSource.Source.InterAction();
                PlayerEnergyM.CurrentSource = SourceDatasList[SourceCount - 1];
                PlayerEnergyM.OnSourceChangedDebug();
            }
            else 
                SourceDatasList[SourceCount - 1].Source.InterAction();
            
            if (nodes[SourceCount - 1].dendrite is DentriteType.Receptor)
                nodes[SourceCount - 1].receptorRef.OnReset();
            SourceDatasList.RemoveAt(SourceCount - 1);
            base.PlayerCancel();
        }

        #endregion

        #region fields
        
        [SerializeField] private NodeDatas[] nodes;

        #endregion
    }
}