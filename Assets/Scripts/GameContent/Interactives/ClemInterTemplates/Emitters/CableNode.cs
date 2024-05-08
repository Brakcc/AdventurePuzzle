using TMPro;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public class CableNode : BaseInterBehavior
    {
        #region properties

        public short CurrentOrientationLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                levelText.text = _currentLevel.ToString();
            }
        }

        #endregion

        #region fields

        [SerializeField] private TMP_Text levelText;    
        
        private short _currentLevel;

        #endregion
    }
}