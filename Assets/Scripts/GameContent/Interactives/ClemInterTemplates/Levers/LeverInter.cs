using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Levers
{
    public abstract class LeverInter : BaseInterBehavior
    {
        #region properties

        public abstract sbyte Level { get; set; }

        public LeverOrientationMode LeverOrientationMode => leverOrientation;

        #endregion
        
        #region methodes

        protected override void OnInit()
        {
            base.OnInit();
            debugTextLocal = "Maintain <b>E</b> to interact";
        }

        public override void PlayerAction()
        {
            //Debug.Log($"player action {this}");
        }

        public override void InterAction()
        {
            //Debug.Log($"inter action {this}");
        }
        
        #endregion

        #region fields
        
        [SerializeField] private LeverOrientationMode leverOrientation; 
        
        [SerializeField] private GameObject imageIndic;
        
        [SerializeField] private GameObject imageVert;
        
        protected sbyte _currentLevel;

        #endregion
    }

    public enum LeverOrientationMode
    {
        Horizontal = 0,
        Vertical = 1
    }
}