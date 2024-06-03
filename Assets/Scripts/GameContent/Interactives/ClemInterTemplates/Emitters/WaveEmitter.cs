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
        
        public sbyte CurrentHeightLevel
        {
            get => _currentLevel;
            set
            {
                var prevLevel = _currentLevel;
                _currentLevel = value;
                //datas.levelText.text = _currentLevel.ToString();
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
                r.ReceptorInter.EmitsRef.Add(this);
            }
            recepDatas.Sort(Compare);
            
            #region VFX

            _matBlock = new MaterialPropertyBlock();
            datas.sphereRend.GetPropertyBlock(_matBlock);
            
            var curve = new AnimationCurve(new Keyframe(0, 0, 2, 3), 
                                           new Keyframe(datas.waveSpeed, datas.maxDistHit));
            datas.monoWave.SetAnimationCurve("curve", curve);
            datas.monoWave.SetFloat("life", datas.waveSpeed);
            
            if (PreSources.Length == 0)
            {
                SwitchMaterial(SourceDatasList);
                return;
            }
            
            var tempSources = new SourceDatas[PreSources.Length];
            for (var i = 0; i < PreSources.Length; i++)
            {
                tempSources[i] = new SourceDatas(PreSources[i]);
            }
            SwitchMaterial(tempSources);
            
            #endregion
        }

        public override void InterAction()
        {
        }

        public override void PlayerAction()
        {
            if (SourceCount >= Constants.MaxWaveEmitterEnergyContaints)
                return;
            
            if (PlayerEnergyM.EnergyType == EnergyTypes.None)
                return;
            
            SourceDatasList.Add(PlayerEnergyM.CurrentSource);
            PlayerEnergyM.CurrentSource = new SourceDatas(null);
            PlayerEnergyM.OnSourceChangedDebug();
            
            recepDatas.Sort(Compare);
            SwitchMaterial(SourceDatasList);
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
            
            SwitchMaterial(SourceDatasList);
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
                #region VFX
                
                var tempGO1 = Instantiate(datas.waveVFXs[0], datas.sphere.position, Quaternion.identity);
                if (tempGO1.TryGetComponent<ParticleSystem>(out var v1))
                {
                    v1.Play();
                    Destroy(tempGO1, v1.main.duration);
                }
                else
                {
                    Destroy(tempGO1);
                }
                
                #endregion
                
                yield return new WaitForSeconds(Constants.MonoWaveBeforeDelay);
                
                #region VFX
                
                if (this[i - 1].Type == EnergyTypes.Green)
                {
                    var tempGO2 = Instantiate(datas.waveVFXs[1], datas.sphere.position, Quaternion.identity);
                    if (tempGO2.TryGetComponent<ParticleSystem>(out var v2))
                    {
                        v2.Play();
                        Destroy(tempGO2, v2.main.duration);
                    }
                    else
                    {
                        Destroy(tempGO2);
                    }
                }

                if (this[i - 1].Type == EnergyTypes.Blue)
                {
                    var tempGO3 = Instantiate(datas.waveVFXs[2], datas.sphere.position, Quaternion.identity);
                    if (tempGO3.TryGetComponent<ParticleSystem>(out var v3))
                    {
                        v3.Play();
                        Destroy(tempGO3, v3.main.duration);
                    }
                    else
                    {
                        Destroy(tempGO3);
                    }
                }
                
                #endregion
                
                var j = RecepCount;
                while (j > 0)
                {
                    StartCoroutine(MonoWaveStarted(i, j));
                    j--;
                }
                i--;
                //who tf are i and j ?
                yield return new WaitForSeconds(datas.monoWaveDelay);
            }
        }

        private IEnumerator MonoWaveStarted(int i, int j)
        {
            if (this[i - 1].Type == EnergyTypes.None)
                yield break;
            
            #region VFX
            
            datas.monoWave.SetVector4("rgba", new Vector4(
                                                          SourceDatas.GetTypedColor(this[i - 1].Type).r * 255,
                                                          SourceDatas.GetTypedColor(this[i - 1].Type).g * 255,
                                                          SourceDatas.GetTypedColor(this[i - 1].Type).b * 255,
                                                          SourceDatas.GetTypedColor(this[i - 1].Type).a * 0.2f));

            //datas.monoWave.Play();
            
            #endregion
            
            yield return new WaitForSeconds((recepDatas[j - 1].ReceptorInter.GetDistFromEmit(this) + recepDatas[j - 1].ActivationDelay) / datas.waveSpeed);
            
            if (recepDatas[j - 1].ReceptorInter.GetDistFromEmit(this) >= datas.maxDistHit ||
                Mathf.Abs(datas.sphere.position.y + datas.levelCorrector - recepDatas[j - 1].ReceptorInter.Pivot.y) >=
                datas.inBetweenLevelThreshold / 2 + datas.ampliCorrector || 
                recepDatas[j - 1].ReceptorInter.HasCableEnergy)
                yield break;
            
            recepDatas[j - 1].ReceptorInter.HasWaveEnergy = true;
            recepDatas[j - 1].ReceptorInter.CurrentEnergyType = this[i - 1].Type;
        }

        #region VFX Mats
        
        private void SwitchMaterial(IReadOnlyList<SourceDatas> energies)
        {
            switch (energies.Count)
            {
                case 0:
                    _matBlock.SetFloat(TwoColor, 0);
                    _matBlock.SetFloat(ThreeColor, 0);
                    _matBlock.SetColor(Color01, GetEnergyColor(EnergyTypes.None));
                    break;
                case 1:
                    _matBlock.SetFloat(TwoColor, 0);
                    _matBlock.SetFloat(ThreeColor, 0);
                    _matBlock.SetColor(Color01, GetEnergyColor(energies[0].Type));
                    break;
                case 2:
                    _matBlock.SetFloat(TwoColor, 1);
                    _matBlock.SetFloat(ThreeColor, 0);
                    _matBlock.SetColor(Color01, GetEnergyColor(energies[0].Type));
                    _matBlock.SetColor(Color02, GetEnergyColor(energies[1].Type));
                    break;
                case >= 3:
                    _matBlock.SetFloat(TwoColor, 1);
                    _matBlock.SetFloat(ThreeColor, 1);
                    _matBlock.SetColor(Color01, GetEnergyColor(energies[0].Type));
                    _matBlock.SetColor(Color02, GetEnergyColor(energies[1].Type));
                    _matBlock.SetColor(Color03, GetEnergyColor(energies[2].Type));
                    break;
            }
            datas.sphereRend.SetPropertyBlock(_matBlock);
        }

        private static Color GetEnergyColor(EnergyTypes type) => type switch
        {
            EnergyTypes.None => Color.black,
            EnergyTypes.Yellow => Color.yellow,
            EnergyTypes.Blue => new Color(0, 158/255f, 1),
            EnergyTypes.Green => new Color(0, 219/255f, 119/255f),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "ta race")
        };
        
        #endregion
        
        protected override void ForceAbsorbSources(EnergySourceInter[] sources)
        {
            if (sources.Length <= 0)
                return;

            foreach (var s in sources)
            {
                if (SourceCount >= recepDatas.Count)
                    break;
                
                SourceDatasList.Add(new SourceDatas(s));
                s.OnForceAbsorb();
            }
        }

        #region Gizmos
        
        private void OnDrawGizmos()
        {
            Gizmos.color = datas.gizmosColor;
            var position = datas.sphere.position;
            
            Gizmos.DrawWireSphere(position, datas.maxDistHit);
            Gizmos.DrawWireCube(position + Vector3.up * datas.levelCorrector, 
                            new Vector3(datas.maxDistHit * 1.75f, datas.inBetweenLevelThreshold + 2 * datas.ampliCorrector, datas.maxDistHit * 1.75f));

            Gizmos.color = new Color(datas.gizmosColor.a, datas.gizmosColor.g, datas.gizmosColor.b, datas.gizmosColor.a * 0.2f);
            if (!datas.drawDebugCube)
                return;
            Gizmos.DrawCube(position + Vector3.up * datas.levelCorrector, 
                            new Vector3(datas.maxDistHit * 1.75f, datas.inBetweenLevelThreshold + 2 * datas.ampliCorrector, datas.maxDistHit * 1.75f));
        }

        #endregion
        
        #endregion

        #region fields
        
        [SerializeField] private List<RecepDatas> recepDatas;

        [SerializeField] private WaveEmitterDatas datas;
        
        private readonly Comparison<RecepDatas> Compare = (a, b) =>
            Mathf.RoundToInt(Mathf.Sign(a.ActivationDelay /*+ a.ReceptorInter.DistFromEmit*/ -
                            (b.ActivationDelay /*+ b.ReceptorInter.DistFromEmit*/)));
        
        private sbyte _currentLevel;
        
        private float _tripleWavesDelayCounter;
        
        private MaterialPropertyBlock _matBlock;
        
        private static readonly int Color01 = Shader.PropertyToID("_Color01");
        
        private static readonly int Color02 = Shader.PropertyToID("_Color_02");
        
        private static readonly int Color03 = Shader.PropertyToID("_Color_03");
        
        private static readonly int TwoColor = Shader.PropertyToID("_2Couleurs");

        private static readonly int ThreeColor = Shader.PropertyToID("_3Couleurs");

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
        
        [FieldColorLerp(-2, 2)]
        [Range(-2, 2)] [SerializeField] internal float levelCorrector; //yPosSphereCorrector

        [FieldCompletion]
        [SerializeField] internal VisualEffect monoWave;

        [SerializeField] internal GameObject[] waveVFXs;

        [SerializeField] internal Renderer sphereRend;
        
        [SerializeField] internal bool drawDebugCube;
        [SerializeField] internal Color gizmosColor;
        
        #endregion
    }
}