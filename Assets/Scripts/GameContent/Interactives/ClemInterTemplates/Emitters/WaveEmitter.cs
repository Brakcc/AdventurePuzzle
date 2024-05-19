using System;
using System.Collections;
using System.Collections.Generic;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using GameContent.PlayerScripts;
using TMPro;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class WaveEmitter : EmitterInter
    {
        #region properties

        private int RecepCount => recepDatas.Count;

        public Vector3 SpherePos => sphere.position;
        
        public short CurrentHeightLevel
        {
            get => _currentLevel;
            set
            {
                var prevLevel = _currentLevel;
                _currentLevel = value;
                levelText.text = _currentLevel.ToString();
                sphere.position += Vector3.up * (inBetweenLevelThreshold * (_currentLevel - prevLevel));
            }
        }

        #endregion
        
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
            
            SourceDatasList.RemoveAt(SourceCount - 1);
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
            
            if (SourceCount == 0)
                return;
                
            StartCoroutine(WaveStarted());
        }

        private IEnumerator WaveStarted()
        {
            var i = SourceCount;
            while (i > 0)
            {
                var j = RecepCount;
                while (j > 0)
                {
                    StartCoroutine(MonoWaveStarted(i, j));
                    j--;
                }
                i--;

                yield return new WaitForSeconds(monoWaveDelay);
            }
        }

        private IEnumerator MonoWaveStarted(int i, int j)
        {
            yield return new WaitForSeconds((recepDatas[j - 1].ReceptorInter.DistFromEmit + recepDatas[j - 1].ActivationDelay) / waveSpeed);
            
            if (recepDatas[j - 1].ReceptorInter.DistFromEmit >= maxDistHit ||
                Mathf.Abs(sphere.position.y + levelCorrector - recepDatas[j - 1].ReceptorInter.Pivot.y) >=
                inBetweenLevelThreshold / 2 + ampliCorrector)
                yield break;
            
            recepDatas[j - 1].ReceptorInter.CurrentEnergyType = this[i - 1].Type;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawWireSphere(sphere.position, maxDistHit);
            Gizmos.DrawWireCube(sphere.position + Vector3.up * levelCorrector, 
                            new Vector3(maxDistHit * 1.75f, inBetweenLevelThreshold + 2 * ampliCorrector, maxDistHit * 1.75f));

            Gizmos.color = new Color(gizmosColor.a, gizmosColor.g, gizmosColor.b, gizmosColor.a * 0.2f);
            if (!drawDebugCube)
                return;
            Gizmos.DrawCube(sphere.position + Vector3.up * levelCorrector, 
                            new Vector3(maxDistHit * 1.75f, inBetweenLevelThreshold + 2 * ampliCorrector, maxDistHit * 1.75f));
        }

        #endregion

        #region fields
        
        [SerializeField] private List<RecepDatas> recepDatas;
        
        [FieldCompletion]
        [SerializeField] private Transform sphere;
        
        [FieldCompletion(_uncheckedColor:FieldColor.Orange)]
        [SerializeField] private TMP_Text levelText;

        [FieldColorLerp(1, 10)]
        [Range(1, 10)] [SerializeField] private float tripleWavesDelay;
        
        [FieldColorLerp(1, 10)]
        [Range(1, 10)] [SerializeField] private float monoWaveDelay;
        
        [FieldColorLerp(1, 10)]
        [Range(1, 10)] [SerializeField] private float waveSpeed;

        [FieldColorLerp(1, 30)]
        [Range(1, 30)] [SerializeField] private float maxDistHit;
        
        [FieldColorLerp(0, 3)]
        [Range(0, 3)] [SerializeField] private float inBetweenLevelThreshold;

        [FieldColorLerp(0, 1)]
        [Range(0, 1)] [SerializeField] private float ampliCorrector;
        
        [FieldColorLerp(0, 1)]
        [Range(0, 1)] [SerializeField] private float levelCorrector; //yPosSphereCorrector

        [SerializeField] private bool drawDebugCube;
        [SerializeField] private Color gizmosColor;

        private readonly Comparison<RecepDatas> Compare = (a, b) =>
            Mathf.RoundToInt(Mathf.Sign(a.ActivationDelay + a.ReceptorInter.DistFromEmit -
                            (b.ActivationDelay + b.ReceptorInter.DistFromEmit)));
        
        private short _currentLevel;
        
        private float _tripleWavesDelayCounter;

        #endregion
    }
}