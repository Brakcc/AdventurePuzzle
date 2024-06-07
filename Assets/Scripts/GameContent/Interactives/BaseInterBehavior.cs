using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;

namespace GameContent.Interactives
{
    public abstract class BaseInterBehavior : MonoBehaviour
    {
        #region properties
        
        public float DistFromPlayer { get; private set; }
        
        public float AngleWithPlayer { get; private set; }

        protected bool HasCheckerRef => _checkerRef is not null;

        #endregion

        #region constructors

        ~BaseInterBehavior()
        {
            //Debug.Log($"{gameObject.name} removed");
        }

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
            OnUpdate();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate();
        }

        public void AddSelf(InterCheckerState checker)
        {
            _isInRange = true;
            _checkerRef = checker;
            _checkerRef.InRangeInter.Add(this);
        }

        public void RemoveSelf()
        {
            _isInRange = false;
            _checkerRef.InRangeInter.Remove(this);
            _checkerRef = null;
        }
        
        #region Methodes a hériter
        
        protected virtual void OnInit() {}

        protected virtual void OnUpdate()
        {
            if (!_isInRange) 
                return;

            var localPos = transform.position;
            var playerPos = _checkerRef.transform.position;
            
            DistFromPlayer = Vector3.Distance(localPos, playerPos);

            var vecPlayerToTrans = localPos - playerPos;
            AngleWithPlayer = Vector3.Angle(vecPlayerToTrans, _checkerRef.transform.forward);
        }

        protected virtual void OnFixedUpdate() {}
        
        public virtual void PlayerAction() {}

        public virtual void PlayerCancel() {}

        public virtual void InterAction() {}
    
        #endregion
        
        #endregion
        
        #region fields
        
        private InterCheckerState _checkerRef;
        
        private bool _isInRange;

        protected bool isActivated;

        protected string debugTextLocal;

        #endregion
    }
}