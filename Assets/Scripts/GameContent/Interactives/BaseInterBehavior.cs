using GameContent.PlayerScripts.PlayerStates;
using TMPro;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives
{
    public abstract class BaseInterBehavior : MonoBehaviour
    {
        #region properties

        public float DistFromPlayer { get; private set; }

        #endregion
        
        #region methodes

        private void Awake()
        {
            _isInRange = false;
            isActivated = true;
            
            OnInit();
        }

        private void Update()
        {
            if (!_isInRange) 
                return;

            DistFromPlayer = Vector3.Distance(transform.position, _checkerRef.transform.position);
            
            // if (DistFromPlayer <= maxDistFromPlayer)
            //     return;
            //
            // RemoveSelf();
        }

        public void AddSelf(InterCheckerState checker)
        {
            debugInputText.enabled = true;
            debugInputText.text = debugText;
            _isInRange = true;
            _checkerRef = checker;
            _checkerRef.InRangeInter.Add(this);
        }

        public void RemoveSelf()
        {
            //Debug.Log($"{name} removed");
            debugInputText.enabled = false;
            _isInRange = false;
            _checkerRef.InRangeInter.Remove(this);
            _checkerRef = null;
        }
        
        #region Methodes a hériter
        
        protected virtual void OnInit() {}

        public abstract void PlayerAction();

        public virtual void PlayerCancel() {}

        public abstract void InterAction();
    
        #endregion
        
        #endregion
        
        #region fields

        [FieldCompletion(FieldColor.Blue, FieldColor.Cyan)] [SerializeField]
        private TMP_Text debugInputText;

        protected string debugText;
        
        [FieldColorLerp(FieldColor.Green, FieldColor.Blue,0, 10)]
        [Range(0, 10)] [SerializeField] private float maxDistFromPlayer;
        
        private InterCheckerState _checkerRef;
        
        protected bool isActivated;

        private bool _isInRange;

        #endregion
    }
}