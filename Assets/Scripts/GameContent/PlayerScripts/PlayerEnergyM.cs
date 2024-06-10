using System;
using DebuggingClem;
using GameContent.Interactives.ClemInterTemplates;
using UnityEngine;

namespace GameContent.PlayerScripts
{
    public class PlayerEnergyM : MonoBehaviour
    {
        #region properties
        
        public static SourceDatas CurrentSource { get; set; }

        public static EnergyTypes EnergyType => CurrentSource.Type;

        private static Light PlayerLight { get; set; }

        public static bool GetEnergyBack { get; private set; }

        #endregion
        
        #region methodes

        private void Start()
        {
            CurrentSource = new SourceDatas(null);
            PlayerLight = GetComponentInChildren<Light>();
            GetEnergyBack = getEnergyBack;
            OnSourceChangedDebug();

            _matBlock = new MaterialPropertyBlock();
            foreach (var t in hairRend)
            {
                t.GetPropertyBlock(_matBlock);
            }
        }

        private void Update()
        {
            if (EnergyType is EnergyTypes.Blue && _hairLerpCoef < 1f)
            {
                _hairLerpCoef += Time.fixedDeltaTime;

                if (_hairLerpCoef > 0 && Mathf.Approximately(_hairGreenOn, 1))
                {
                    _hairGreenOn = 0;
                    _matBlock.SetFloat(GreenE, Mathf.RoundToInt(_hairGreenOn));
                }

                _matBlock.SetFloat(FadeE, Mathf.Abs(_hairLerpCoef));
                foreach (var r in hairRend)
                {
                    r.SetPropertyBlock(_matBlock);
                }
            }

            if (EnergyType is EnergyTypes.Green && _hairLerpCoef > -1f)
            {
                _hairLerpCoef -= Time.fixedDeltaTime;

                if (_hairLerpCoef < 0 && Mathf.Approximately(_hairGreenOn, 0))
                {
                    _hairGreenOn = 1;
                    _matBlock.SetFloat(GreenE, Mathf.RoundToInt(_hairGreenOn));
                }

                _matBlock.SetFloat(FadeE, Mathf.Abs(_hairLerpCoef));
                foreach (var r in hairRend)
                {
                    r.SetPropertyBlock(_matBlock);
                }
            }

            if (EnergyType is EnergyTypes.None && Mathf.Abs(_hairLerpCoef) > 0f)
            {
                switch (_hairLerpCoef)
                {
                    case > 0f:
                        _hairLerpCoef -= Time.fixedDeltaTime;
                        break;
                    case < 0f:
                        _hairLerpCoef += Time.fixedDeltaTime;
                        break;
                }

                if (Mathf.Abs(_hairLerpCoef) is < 0.01f and > 0f)
                    _hairLerpCoef = 0;

                _matBlock.SetFloat(FadeE, Mathf.Abs(_hairLerpCoef));
                foreach (var r in hairRend)
                {
                    r.SetPropertyBlock(_matBlock);
                }
            }
        }

        public static void OnSourceChangedDebug() => PlayerLight.color = LightDebugger.DebugColor(EnergyType);

        #endregion

        #region fields

        [SerializeField] private bool getEnergyBack;

        [SerializeField] private Renderer[] hairRend;

        private MaterialPropertyBlock _matBlock;
        
        private float _hairLerpCoef;

        private float _hairGreenOn;
        
        private static readonly int GreenE = Shader.PropertyToID("_On_Green_Off_Blue");

        private static readonly int FadeE = Shader.PropertyToID("_On_Energy_fade");

        #endregion
    }
}