using System;
using Cinemachine;
using GameContent.Interactives.ClemInterTemplates.Emitters;
using UIScripts.Sounds;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Levers
{
    public sealed class CableOrienteer : LeverInter
    {
        #region properties

        private void Start()
        {
            _value = Level;
        }

        public override sbyte Level
        {
            get => _currentLevel;
            set
            {
                _currentLevel = (sbyte)(value < 0 ? value + Constants.OrientationNumber : value % Constants.OrientationNumber);
                PlayerAction();
            }
        }

        private Transform DistributorPivot => distributorRef.Pivot;
        
        #endregion
        
        #region methodes

        protected override void OnInit()
        {
            base.OnInit();
            Level = distributorRef.StartingLevel;
        }

        public override void PlayerAction()
        {
            base.PlayerAction();
            distributorRef.CurrentOrientationLevel = Level;

            if ((_value < Level || (_value == 3 && Level == 0)) && (!(_value == 0 && Level == 3)))
            {
                transform.GetChild(2).GetComponent<PlaySound>().PlayMySound();
            }
            else
            {
                transform.GetChild(1).GetComponent<PlaySound>().PlayMySound();
            }
            _value = Level;
            
            UnMaxDanimAAjouter();
        }
        
        private void UnMaxDanimAAjouter()
        {
            //arreter d'avoir la flemme et faire le truc
            
            DistributorPivot.rotation = Quaternion.Euler(0, GetDistributorAngle(), 0);
        }

        private float GetDistributorAngle() => Level switch
        {
            0 => distributorRef.BaseYRota + 0,
            1 => distributorRef.BaseYRota + 90,
            2 => distributorRef.BaseYRota + 180,
            3 => distributorRef.BaseYRota + 270,
            _ => distributorRef.BaseYRota + 0
        };
        
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawLine(transform.position, distributorRef.transform.position);
        }

        #endregion

        #region fields

        [SerializeField] private Distributor distributorRef;

        [SerializeField] private Color gizmosColor;

        private int _value;

        #endregion
    }
}