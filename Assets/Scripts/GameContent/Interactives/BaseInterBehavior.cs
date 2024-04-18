using DebuggingClem;
using GameContent.PlayerScripts.PlayerStates;
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
            if (hasDebugMod)
            {
                Debug.Log($"{name} added");
                debugMod.debugText.enabled = true;
                debugMod.debugText.text = debugTextLocal;
            }
            _isInRange = true;
            _checkerRef = checker;
            _checkerRef.InRangeInter.Add(this);
        }

        public void RemoveSelf()
        {
            if (hasDebugMod)
            {
                Debug.Log($"{name} removed");
                debugMod.debugText.enabled = false;
            }
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

        [SerializeField] protected bool hasDebugMod;
        [ShowIfBoolTrue("hasDebugMod")] [SerializeField] protected DebugModDatas debugMod;
        
        [FieldColorLerp(FieldColor.Green, FieldColor.Blue,0, 10)]
        [Range(0, 10)] [SerializeField] private float maxDistFromPlayer;
        
        private InterCheckerState _checkerRef;
        
        protected bool isActivated;

        private bool _isInRange;

        protected string debugTextLocal;

        #endregion
    }
}