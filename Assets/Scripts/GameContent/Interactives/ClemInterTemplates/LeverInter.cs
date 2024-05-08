using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates
{
    public sealed class LeverInter : BaseInterBehavior
    {
        #region properties

        public GameObject ImageD => imageIndic;

        public GameObject ImageF => imageVert;

        public short Level
        {
            get => _currentLevel;
            set
            {
                _currentLevel = (short)(value < 0 ? value + levelNumbers : value % levelNumbers);
                PlayerAction();
            }
        }

        public LeverOrientationMode LeverOrientationMode => leverOrientation;

        #endregion
        
        #region methodes

        protected override void OnInit()
        {
            base.OnInit();
            debugTextLocal = "Maintain <b>E</b> to interact";
            Level = 0;
        }

        public override void PlayerAction()
        {
            //Debug.Log($"player action {this}");
            emitRef.EmitLevel = Level;
        }

        public override void InterAction()
        {
            //Debug.Log($"inter action {this}");
        }
        
        #endregion

        #region fields

        [SerializeField] private EmitterInter emitRef;
        
        [FieldColorLerp(FieldColor.Red, FieldColor.Green, 5, 0)]
        [Range(0, 5)] [SerializeField] private short levelNumbers;

        [SerializeField] private LeverOrientationMode leverOrientation; 
        
        [SerializeField] private GameObject imageIndic;
        
        [SerializeField] private GameObject imageVert;
        
        private short _currentLevel;

        #endregion
    }

    public enum LeverOrientationMode
    {
        Horizontal = 0,
        Vertical = 1
    }
}