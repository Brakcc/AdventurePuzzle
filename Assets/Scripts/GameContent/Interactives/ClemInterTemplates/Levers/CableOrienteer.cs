﻿using GameContent.Interactives.ClemInterTemplates.Emitters;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Levers
{
    public sealed class CableOrienteer : LeverInter
    {
        #region properties

        public override short Level
        {
            get => _currentLevel;
            set
            {
                _currentLevel = (short)(value < 0 ? value + Constants.OrientationNumber : value % Constants.OrientationNumber);
                PlayerAction();
            }
        }

        #endregion
        
        #region methodes

        public override void PlayerAction()
        {
            base.PlayerAction();
            nodeRef.CurrentOrientationLevel = Level;
            UnMaxDanimAAjouter();
        }

        private void UnMaxDanimAAjouter()
        {
            
        }

        #endregion

        #region fields

        [SerializeField] private CableNode nodeRef;

        #endregion
    }
}