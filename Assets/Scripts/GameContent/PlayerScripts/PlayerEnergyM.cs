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

        #endregion
        
        #region methodes
        
        private void Start()
        {
            CurrentSource = new SourceDatas(null);
            PlayerLight = GetComponentInChildren<Light>();
            OnSourceChangedDebug();
        }

        public static void OnSourceChangedDebug() => PlayerLight.color = LightDebugger.DebugColor(EnergyType);

        #endregion
    }
}