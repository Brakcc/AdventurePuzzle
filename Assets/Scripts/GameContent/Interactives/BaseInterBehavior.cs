using DebuggingClem;
using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.Interactives
{
    public abstract class BaseInterBehavior : MonoBehaviour
    {
        #region properties
        
        public float DistFromPlayer { get; private set; }
        
        public float AngleWithPlayer { get; private set; }

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

            var localPos = transform.position;
            var playerPos = _checkerRef.transform.position;
            
            DistFromPlayer = Vector3.Distance(localPos, playerPos);

            var vecPlayerToTrans = localPos - playerPos;
            AngleWithPlayer = Vector3.Angle(vecPlayerToTrans, _checkerRef.transform.forward);
        }

        public void AddSelf(InterCheckerState checker)
        {
            if (hasDebugMod)
            {
                //Debug.Log($"{name} added");
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
                //Debug.Log($"{name} removed");
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
        
        private InterCheckerState _checkerRef;
        
        private bool _isInRange;

        protected bool isActivated;

        protected string debugTextLocal;

        #endregion
    }
}