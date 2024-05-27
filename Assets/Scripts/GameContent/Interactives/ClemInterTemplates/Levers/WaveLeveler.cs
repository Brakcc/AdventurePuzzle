using GameContent.Interactives.ClemInterTemplates.Emitters;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates.Levers
{
    public sealed class WaveLeveler : LeverInter
    {
        #region properties

        public override sbyte Level
        {
            get => _currentLevel;
            set
            {
                _currentLevel = (sbyte)(value < 0 ? value + levelNumbers : value % levelNumbers);
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

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawLine(transform.position, emitterRef.SpherePos);
        }

        #endregion
        
        #region fields

        [SerializeField] private WaveEmitter emitterRef;
        
        [FieldColorLerp(FieldColor.Red, FieldColor.Green, 0, 5)]
        [Range(0, 5)] [SerializeField] private sbyte levelNumbers;

        [SerializeField] private Color gizmosColor;

        #endregion
    }
}