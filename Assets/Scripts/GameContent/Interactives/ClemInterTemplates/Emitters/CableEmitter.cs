using System;
using System.Collections;
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

            _canInteract = true;
            
            if (nodes.Length == 0)
                return;
            
            foreach (var n in nodes)
            {
                switch (n.dendrite)
                {
                    case DentriteType.Receptor:
                        n.receptorRef.EmitsRef.Add(this);
                        break;
                    case DentriteType.Distributor:
                        n.distributorRef.SetRef(this);
                        break;
                    case DentriteType.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n.dendrite, "ta ...");
                }
            }
            
            return;
            
            #region VFX
            
            _matBlocks = new MaterialPropertyBlock[]
            {
                new(),
                new(),
                new(),
                new()
            };
            for (var i = 0; i < _matBlocks.Length; i++)
            {
                datas.symbolRends[i].GetPropertyBlock(_matBlocks[i]);
            }
            
            #endregion
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            if (!_canInteract)
                _actionBlockerThreshold += Time.deltaTime;

            if (_actionBlockerThreshold >= Constants.ActionBlockerThreshold)
                _canInteract = true;
        }

        public override void InterAction()
        {
            for (var i = 0; i < SourceCount; i++)
            {
                switch (nodes[i].dendrite)
                {
                    case DentriteType.Receptor:
                        nodes[i].receptorRef.HasCableEnergy = true;
                        nodes[i].receptorRef.HasWaveEnergy = false;
                        nodes[i].receptorRef.CurrentEnergyType = this[i].Type;
                        break;
                    case DentriteType.Distributor:
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
            if (!_canInteract)
                return;

            _canInteract = false;
            
            if (SourceCount >= nodes.Length)
                return;
            
            if (PlayerEnergyM.EnergyType == EnergyTypes.None)
                return;
            
            SourceDatasList.Add(PlayerEnergyM.CurrentSource);
            PlayerEnergyM.CurrentSource = new SourceDatas();
            PlayerEnergyM.OnSourceChangedDebug();

            var tempVFX = SourceDatasList[SourceCount - 1].Type is EnergyTypes.Green
                ? datas.greenPartSys[SourceCount - 1]
                : datas.bluePartSys[SourceCount - 1];
            OnPartLive(tempVFX);
            
            InterAction();
        }

        public override void PlayerCancel()
        {
            if (!_canInteract)
                return;

            _canInteract = false;
            
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
                case DentriteType.Receptor when !nodes[SourceCount - 1].receptorRef.HasWaveEnergy || nodes[SourceCount - 1].receptorRef.HasCableEnergy:
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
            
            var tempVFX = SourceDatasList[SourceCount - 1].Type is EnergyTypes.Green
                ? datas.greenPartSys[SourceCount - 1]
                : datas.bluePartSys[SourceCount - 1];
            OnPartDeath(tempVFX);
            
            SourceDatasList.RemoveAt(SourceCount - 1);
            base.PlayerCancel();
        }

        #region VFX Parts
        
        private static void OnPartLive(ParticleSystem part)
        {
            part.Play();
            part.Pause();
        }

        private static void OnPartDeath(ParticleSystem part)
        {
            part.time = Constants.VFXDatas.BatteryPartLifeSpan;
            part.Play();
        }
        
        #endregion

        protected override void ForceAbsorbSources(EnergySourceInter[] sources)
        {
            if (sources.Length <= 0)
                return;
            
            foreach (var s in sources)
            {
                if (SourceCount >= nodes.Length)
                    break;
                
                SourceDatasList.Add(new SourceDatas(s));
                s.OnForceAbsorb();
                
                var tempVFX = SourceDatasList[SourceCount - 1].Type is EnergyTypes.Green
                    ? datas.greenPartSys[SourceCount - 1]
                    : datas.bluePartSys[SourceCount - 1];
                OnPartLive(tempVFX);
            }
            InterAction();
        }

        #endregion

        #region fields
        
        [SerializeField] private NodeDatas[] nodes;

        [SerializeField] private CableEmitterDatas datas;
        
        private MaterialPropertyBlock[] _matBlocks;

        private float _actionBlockerThreshold;

        private bool _canInteract;

        #endregion
    }

    [Serializable]
    internal class CableEmitterDatas
    {
        #region fields
        
        [SerializeField] internal Renderer[] symbolRends;
        
        [SerializeField] internal Renderer[] symbolGreenVFX;
        
        [SerializeField] internal Renderer[] symbolBlueVFX;

        [SerializeField] internal ParticleSystem[] bluePartSys;
        
        [SerializeField] internal ParticleSystem[] greenPartSys;

        #endregion
    }
}