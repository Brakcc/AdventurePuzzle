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

        protected override void OnInit()
        {
            base.OnInit();
            Level = distributorRef.StartingLevel;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            if (_canInteract)
                return;

            _actionBlockerThreshold += Time.deltaTime;
            
            if (_actionBlockerThreshold <= 0.3f)
                return;

            _actionBlockerThreshold = 0;
            _canInteract = true;
        }

        public override void PlayerAction()
        {
            if (!_canInteract)
                return;

            _canInteract = false;
            
            base.PlayerAction();
            distributorRef.CurrentOrientationLevel = Level;
            UnMaxDanimAAjouter();
        }
        
        private void UnMaxDanimAAjouter()
        {
            //arreter d'avoir la flemme et faire le truc
            
            DistributorPivot.rotation = Quaternion.Euler(0, GetDistributorAngle(), 0);
            
            anim.SetTrigger(Turn);
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

        [SerializeField] private Animator anim;
        
        private static readonly int Turn = Animator.StringToHash("Turn");
        
        [SerializeField] private Distributor distributorRef;

        [SerializeField] private Color gizmosColor;

        private bool _canInteract = true;

        private float _actionBlockerThreshold;

        #endregion
    }
}