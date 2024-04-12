using GameContent.Interactives.ClemInterTemplates;
using UnityEngine;

namespace GameContent.PlayerScripts
{
    public class PlayerEnergyM : MonoBehaviour
    {
        #region propoerties
        
        public static SourceDatas CurrentSource { get; set; }

        public static EnergyTypes EnergyType => CurrentSource.Type;

        #endregion
        
        #region methodes
        
        private void Start() => CurrentSource = new SourceDatas(null);
        
        #endregion
    }
}