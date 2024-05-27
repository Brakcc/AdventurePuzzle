using GameContent.Interactives.ClemInterTemplates.Emitters;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Levers
{
    public sealed class CableOrienteer : LeverInter
    {
        #region properties

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

        public override void PlayerAction()
        {
            base.PlayerAction();
            distributorRef.CurrentOrientationLevel = Level;
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

        #endregion
    }
}