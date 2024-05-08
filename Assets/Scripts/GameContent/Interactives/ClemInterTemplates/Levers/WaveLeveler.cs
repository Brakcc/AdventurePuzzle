using GameContent.Interactives.ClemInterTemplates.Emitters;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates.Levers
{
    public sealed class WaveLeveler : LeverInter
    {
        #region properties

        public override short Level
        {
            get => _currentLevel;
            set
            {
                _currentLevel = (short)(value < 0 ? value + levelNumbers : value % levelNumbers);
                PlayerAction();
            }
        }

        #endregion
        
        #region methodes

        public override void PlayerAction()
        {
            base.PlayerAction();
            emitterRef.CurrentHeightLevel = Level;
        }

        #endregion
        
        #region fields

        [SerializeField] private WaveEmitter emitterRef;
        
        [FieldColorLerp(FieldColor.Red, FieldColor.Green, 5, 0)]
        [Range(0, 5)] [SerializeField] private short levelNumbers;

        #endregion
    }
}