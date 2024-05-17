using System;
using System.Collections.Generic;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using GameContent.PlayerScripts;
using TMPro;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class WaveEmitter : EmitterInter
    {
        #region properties

        public short CurrentHeightLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                levelText.text = _currentLevel.ToString();
            }
        }

        #endregion
        
        #region methodes

        protected override void OnInit()
        {
            base.OnInit();
            _currentEnergySourceID = 0;
            
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
            if (SourceDatasList.Count >= recepDatas.Count)
                return;
            
            if (PlayerEnergyM.EnergyType == EnergyTypes.None)
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

            if (PlayerEnergyM.GetEnergyBack)
            {
                if (PlayerEnergyM.EnergyType != EnergyTypes.None)
                    PlayerEnergyM.CurrentSource.Source.InterAction();
                PlayerEnergyM.CurrentSource = SourceDatasList[Count - 1];
                PlayerEnergyM.OnSourceChangedDebug();
            }
            else
                SourceDatasList[Count - 1].Source.InterAction();
            
            SourceDatasList.RemoveAt(Count - 1);
            base.PlayerCancel();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            if (_tripleWavesDelayCounter <= tripleWavesDelay)
            {
                _tripleWavesDelayCounter += Time.deltaTime;
                return;
            }
            _tripleWavesDelayCounter = 0;
            
            if (Count == 0)
                return;
            
            WaveStarted(this[_currentEnergySourceID].Type);
            _currentEnergySourceID = (short)((_currentEnergySourceID + 1) % Count);
        }

        private async void WaveStarted(EnergyTypes type)
        {
            //var tempTime = 0f;
            Debug.Log(_currentEnergySourceID);
            
        }
        
        #endregion

        #region fields
        
        [SerializeField] private List<RecepDatas> recepDatas;
        
        [SerializeField] private TMP_Text levelText;

        [Range(1, 10)]
        [SerializeField] private float tripleWavesDelay;
        
        [SerializeField] private float waveSpeed;

        private readonly Comparison<RecepDatas> Compare = (a, b) =>
            Mathf.RoundToInt(Mathf.Sign(a.ActivationDelay + a.ReceptorInter.DistFromEmit -
                            (b.ActivationDelay + b.ReceptorInter.DistFromEmit)));

        private short _currentLevel;

        private short _currentEnergySourceID;
        
        private float _tripleWavesDelayCounter;

        private volatile float _monoWaveDelayCounter; //é_é

        #endregion
    }
}