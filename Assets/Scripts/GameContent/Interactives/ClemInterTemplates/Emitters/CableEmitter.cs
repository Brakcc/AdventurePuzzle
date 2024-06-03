﻿using System;
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
            _actionBlockerThreshold = 0;
            
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
            
            #region VFX

            foreach (var r in datas.stacks)
            {
                r.enabled = false;
            }

            _stackMats = new MaterialPropertyBlock[]
            {
                new(),
                new(),
                new(),
                new()
            };
            for (var i = 0; i < _stackMats.Length; i++)
            {
                datas.stacks[i].GetPropertyBlock(_stackMats[i]);
            }
            
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
            
            SetSymbols();
            
            if (_canInteract)
                return;
            
            _actionBlockerThreshold += Time.deltaTime;

            if (!(_actionBlockerThreshold >= Constants.ActionBlockerThreshold))
                return;
            
            _actionBlockerThreshold = 0;
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

            #region VFX
            
            switch (SourceDatasList[SourceCount - 1].Type)
            {
                case EnergyTypes.Green:
                    StartCoroutine(OnPartLive(datas.greenAppearPartSys, 
                                              datas.stacks[SourceCount - 1],
                                              datas.vFXPos[SourceCount - 1].position, 
                                              SourceDatasList[SourceCount - 1].Type));
                    break;
                case EnergyTypes.Blue:
                    StartCoroutine(OnPartLive(datas.blueAppearPartSys,
                                              datas.stacks[SourceCount - 1],
                                              datas.vFXPos[SourceCount - 1].position, 
                                              SourceDatasList[SourceCount - 1].Type));
                    break;
                case EnergyTypes.None:
                    break;
                case EnergyTypes.Yellow:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            #endregion
            
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

            #region VFX
            
            switch (SourceDatasList[SourceCount - 1].Type)
            {
                case EnergyTypes.Green:
                    OnPartDeath(datas.greenDisapPartSys, datas.stacks[SourceCount - 1], datas.vFXPos[SourceCount - 1].position);
                    break;
                case EnergyTypes.Blue:
                    OnPartDeath(datas.blueDisapPartSys, datas.stacks[SourceCount - 1], datas.vFXPos[SourceCount - 1].position);
                    break;
                case EnergyTypes.None:
                    break;
                case EnergyTypes.Yellow:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            #endregion
            
            SourceDatasList.RemoveAt(SourceCount - 1);
            base.PlayerCancel();
        }

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
                
                #region VFX
                
                switch (SourceDatasList[SourceCount - 1].Type)
                {
                    case EnergyTypes.Green:
                        StartCoroutine(OnPartLive(datas.greenAppearPartSys, 
                                                  datas.stacks[SourceCount - 1], 
                                                  datas.vFXPos[SourceCount - 1].position, 
                                                  SourceDatasList[SourceCount - 1].Type));
                        break;
                    case EnergyTypes.Blue:
                        StartCoroutine(OnPartLive(datas.blueAppearPartSys, 
                                                  datas.stacks[SourceCount - 1], 
                                                  datas.vFXPos[SourceCount - 1].position, 
                                                  SourceDatasList[SourceCount - 1].Type));
                        break;
                    case EnergyTypes.None:
                        break;
                    case EnergyTypes.Yellow:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                #endregion
            }
            InterAction();
        }

        #region VFX Parts
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator OnPartLive(GameObject part, Renderer rend, Vector3 atPos, EnergyTypes type)
        {
            var tempPart = Instantiate(part, atPos, Quaternion.identity);
            var p = tempPart.GetComponentInChildren<ParticleSystem>();
            
            p.Play();
            Destroy(tempPart, Constants.VFXDatas.BetteryAppearLifeSpan);

            yield return new WaitForSeconds(Constants.VFXDatas.BatteryPartLifeSpan);

            rend.enabled = true;
            _stackMats[SourceCount - 1].SetFloat(GreenBlue, type is EnergyTypes.Green ? 1 : 0);
            _matBlocks[SourceCount - 1].SetColor(EmissionColor, GetMeshColor(type));
            
            rend.SetPropertyBlock(_stackMats[SourceCount - 1]);
            datas.symbolRends[SourceCount - 1].SetPropertyBlock(_matBlocks[SourceCount - 1]);
        }

        private static void OnPartDeath(GameObject part, Renderer rend, Vector3 atPos)
        {
            var tempPart = Instantiate(part, atPos, Quaternion.identity);
            var p = tempPart.GetComponentInChildren<ParticleSystem>();
            
            p.Play();
            Destroy(tempPart, Constants.VFXDatas.BatteryDisapLifeSpan);

            rend.enabled = false;
        }

        private void SetSymbols()
        {
            
        }
        
        private static Color GetMeshColor(EnergyTypes type) => type switch
        {
            EnergyTypes.None => Color.white,
            EnergyTypes.Yellow => Color.white,
            EnergyTypes.Green => new Color(0, 135 / 255f, 1, 1),
            EnergyTypes.Blue => new Color(0, 1, 167 / 255f, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        
        #endregion

        #endregion

        #region fields
        
        [SerializeField] private NodeDatas[] nodes;

        [SerializeField] private CableEmitterDatas datas;
        
        private MaterialPropertyBlock[] _matBlocks;

        private MaterialPropertyBlock[] _stackMats;

        private float _actionBlockerThreshold;

        private bool _canInteract;
        
        private static readonly int EmissionImplication = Shader.PropertyToID("_emissionImplication");
        
        private static readonly int EmissionColor = Shader.PropertyToID("_emissionColor");

        private static readonly int GreenBlue = Shader.PropertyToID("_On_Green_Off_Blue");

        #endregion
    }

    [Serializable]
    internal class CableEmitterDatas
    {
        #region fields
        
        [SerializeField] internal Renderer[] symbolRends;

        [SerializeField] internal Transform[] vFXPos;

        [SerializeField] internal Renderer[] stacks;
        
        [SerializeField] internal GameObject blueAppearPartSys;
        
        [SerializeField] internal GameObject greenAppearPartSys;

        [SerializeField] internal GameObject blueDisapPartSys;
        
        [SerializeField] internal GameObject greenDisapPartSys;

        #endregion
    }
}