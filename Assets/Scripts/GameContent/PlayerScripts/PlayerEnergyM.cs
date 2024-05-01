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

        private void Awake()
        {
            //instance = this;
        }

        private void Start()
        {
            CurrentSource = new SourceDatas(null);
            PlayerLight = GetComponentInChildren<Light>();
            GetEnergyBack = getEnergyBack;
            OnSourceChangedDebug();
        }

        public static void OnSourceChangedDebug() => PlayerLight.color = LightDebugger.DebugColor(EnergyType);

        #endregion

        #region fields

        [SerializeField] private bool getEnergyBack;

        //public static PlayerEnergyM instance;

        #endregion
    }
}