using System;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using GameContent.PlayerScripts;
using UnityEngine;

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
            for (var i = 0; i < SourceCount; i++)
            {
                switch (nodes[i].dendrite)
                {
                    case DentriteType.Receptor:
                        nodes[i].receptorRef.HasCableEnergy = true;
                        nodes[i].receptorRef.CurrentEnergyType = this[i].Type;
                        break;
                    case DentriteType.Distributor:
                        Debug.Log($"energy added type {nodes[i].distributorRef.IncomingCollectedEnergy} and from  {this[i].Type} ");
                        nodes[i].distributorRef.IncomingCollectedEnergy = this[i].Type;
                        break;
                    case DentriteType.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(nodes), nodes[i].dendrite, "et bah non");
                }
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

            switch (nodes[SourceCount - 1].dendrite)
            {
                case DentriteType.Receptor:
                    nodes[SourceCount - 1].receptorRef.HasCableEnergy = false;
                    nodes[SourceCount - 1].receptorRef.OnReset();
                    break;
                case DentriteType.Distributor:
                    nodes[SourceCount - 1].distributorRef.IncomingCollectedEnergy = EnergyTypes.None;
                    break;
                case DentriteType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nodes), nodes[SourceCount - 1].dendrite, "et bah non ca passe pas");
            }

            SourceDatasList.RemoveAt(SourceCount - 1);
            base.PlayerCancel();
        }

        #endregion

        #region fields
        
        [SerializeField] private NodeDatas[] nodes;

        #endregion
    }
}