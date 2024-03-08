﻿using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives
{
    public abstract class BaseInterBehavior : MonoBehaviour
    {
        #region properties

        public float DistFromPlayer => _distFromPlayer;

        #endregion
        
        #region methodes

        private void Awake()
        {
            isActivated = false;
            _isInRange = false;
        }

        private void Update()
        {
            if (!_isInRange)
                return;

            _distFromPlayer = Vector3.Distance(transform.position, _checkerRef.transform.position);
            
            if (_distFromPlayer <= maxDistFromPlayer)
                return;
            
            RemoveSelf();
        }

        public void AddSelf(InterCheckerState checker)
        {
            _isInRange = true;
            _checkerRef = checker;
            _checkerRef.InRangeInter.Add(this);
        }

        private void RemoveSelf()
        {
            _isInRange = false;
            _checkerRef.InRangeInter.Remove(this);
            _checkerRef = null;
        }
        
        #region Methodes a hériter
    
        public abstract void OnSubscribe();
        public abstract void OnUnSubscribe();
        protected virtual void Effect() => isActivated = !isActivated; 
    
        #endregion
        
        #endregion
        
        #region fields

        private InterCheckerState _checkerRef;

        [FieldColorLerp(FieldColor.Green, FieldColor.Blue,0, 10)]
        [Range(0, 10)] [SerializeField] private float maxDistFromPlayer;
        
        protected bool isActivated;

        private bool _isInRange;

        private float _distFromPlayer;

        #endregion
    }
}