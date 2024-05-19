using System;
using System.Collections;
using System.Collections.Generic;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using GameContent.PlayerScripts;
using TMPro;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;
using UnityEngine.VFX;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class WaveEmitter : EmitterInter
    {
        #region properties

        private int RecepCount => recepDatas.Count;

        public Vector3 SpherePos => datas.sphere.position;
        
        public short CurrentHeightLevel
        {
            get => _currentLevel;
            set
            {
                var prevLevel = _currentLevel;
                _currentLevel = value;
                datas.levelText.text = _currentLevel.ToString();
                datas.sphere.position += Vector3.up * (datas.inBetweenLevelThreshold * (_currentLevel - prevLevel));
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
            
            var curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(datas.waveSpeed / 2, datas.maxDistHit / 2));
            datas.monoWave.SetAnimationCurve("curve", curve);
            datas.monoWave.SetAnimationCurve("curve", curve);
            datas.monoWave.SetFloat("life", 1);
        }

        public override void InterAction()
        {
            //ton père le conifère
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
            
            if (_tripleWavesDelayCounter <= datas.tripleWavesDelay)
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

                yield return new WaitForSeconds(datas.monoWaveDelay);
            }
        }

        private IEnumerator MonoWaveStarted(int i, int j)
        {
            if (this[i - 1].Type == EnergyTypes.None)
                yield break;
            
            #region VFX
            
            datas.monoWave.SetFloat("r", SourceDatas.GetTypedColor(this[i - 1].Type).r * 255);
            datas.monoWave.SetFloat("g", SourceDatas.GetTypedColor(this[i - 1].Type).g * 255);
            datas.monoWave.SetFloat("b", SourceDatas.GetTypedColor(this[i - 1].Type).b * 255);
            datas.monoWave.SetFloat("a", SourceDatas.GetTypedColor(this[i - 1].Type).a * 0.2f);

            datas.monoWave.Play();
            
            #endregion
            
            yield return new WaitForSeconds((recepDatas[j - 1].ReceptorInter.DistFromEmit + recepDatas[j - 1].ActivationDelay) / datas.waveSpeed);
            
            if (recepDatas[j - 1].ReceptorInter.DistFromEmit >= datas.maxDistHit ||
                Mathf.Abs(datas.sphere.position.y + datas.levelCorrector - recepDatas[j - 1].ReceptorInter.Pivot.y) >=
                datas.inBetweenLevelThreshold / 2 + datas.ampliCorrector)
                yield break;
            
            recepDatas[j - 1].ReceptorInter.CurrentEnergyType = this[i - 1].Type;
        }

        #region Gizmos
        
        private void OnDrawGizmos()
        {
            Gizmos.color = datas.gizmosColor;
            Gizmos.DrawWireSphere(datas.sphere.position, datas.maxDistHit);
            Gizmos.DrawWireCube(datas.sphere.position + Vector3.up * datas.levelCorrector, 
                            new Vector3(datas.maxDistHit * 1.75f, datas.inBetweenLevelThreshold + 2 * datas.ampliCorrector, datas.maxDistHit * 1.75f));

            Gizmos.color = new Color(datas.gizmosColor.a, datas.gizmosColor.g, datas.gizmosColor.b, datas.gizmosColor.a * 0.2f);
            if (!datas.drawDebugCube)
                return;
            Gizmos.DrawCube(datas.sphere.position + Vector3.up * datas.levelCorrector, 
                            new Vector3(datas.maxDistHit * 1.75f, datas.inBetweenLevelThreshold + 2 * datas.ampliCorrector, datas.maxDistHit * 1.75f));
        }

        #endregion
        
        #endregion

        #region fields
        
        [SerializeField] private List<RecepDatas> recepDatas;

        [SerializeField] private WaveEmitterDatas datas;
        
        private readonly Comparison<RecepDatas> Compare = (a, b) =>
            Mathf.RoundToInt(Mathf.Sign(a.ActivationDelay + a.ReceptorInter.DistFromEmit -
                            (b.ActivationDelay + b.ReceptorInter.DistFromEmit)));
        
        private short _currentLevel;
        
        private float _tripleWavesDelayCounter;

        #endregion
    }
    
    [Serializable]
    internal class WaveEmitterDatas
    {
        #region fields
        
        [FieldCompletion]
        [SerializeField] internal Transform sphere;
        
        [FieldCompletion(_uncheckedColor:FieldColor.Orange)]
        [SerializeField] internal TMP_Text levelText;

        [FieldColorLerp(1, 10)]
        [Range(1, 10)] [SerializeField] internal float tripleWavesDelay;
        
        [FieldColorLerp(1, 10)]
        [Range(1, 10)] [SerializeField] internal float monoWaveDelay;
        
        [FieldColorLerp(1, 10)]
        [Range(1, 10)] [SerializeField] internal float waveSpeed;

        [FieldColorLerp(1, 30)]
        [Range(1, 30)] [SerializeField] internal float maxDistHit;
        
        [FieldColorLerp(0, 3)]
        [Range(0, 3)] [SerializeField] internal float inBetweenLevelThreshold;

        [FieldColorLerp(0, 1)]
        [Range(0, 1)] [SerializeField] internal float ampliCorrector;
        
        [FieldColorLerp(0, 1)]
        [Range(0, 1)] [SerializeField] internal float levelCorrector; //yPosSphereCorrector

        [FieldCompletion]
        [SerializeField] internal VisualEffect monoWave;
        
        [SerializeField] internal bool drawDebugCube;
        [SerializeField] internal Color gizmosColor;
        
        #endregion
    }
}