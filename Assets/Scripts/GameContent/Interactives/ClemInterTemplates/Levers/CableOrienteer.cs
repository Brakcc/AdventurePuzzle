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

        private Transform NodeTrans => nodeRef.transform;
        
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
            //arreter d'avoir la flemme et faire le truc
            //Debug.Log($"{nodeRef.CurrentDistribution[0]}  {nodeRef.CurrentDistribution[1]}  {nodeRef.CurrentDistribution[2]}  {nodeRef.CurrentDistribution[3]}");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawLine(transform.position, nodeRef.transform.position);
        }

        #endregion

        #region fields

        [SerializeField] private Distributor nodeRef;

        [SerializeField] private Color gizmosColor;

        #endregion
    }
}