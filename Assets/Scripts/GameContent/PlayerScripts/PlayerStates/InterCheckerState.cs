using System;
using System.Collections.Generic;
using GameContent.Interactives;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class InterCheckerState : MonoBehaviour
    {
        #region properties

        public BaseInterBehavior InterRef { get; private set; }

        public List<BaseInterBehavior> InRangeInter { get; private set; }

        #endregion

        #region methodes

        private void Start()
        {
            InRangeInter = new List<BaseInterBehavior>();
        }

        private void Update()
        {
            switch (InRangeInter.Count)
            {
                case <= 0:
                    return;
                case >= 2:
                    InRangeInter.Sort(CompareInters);
                    break;
            }

            InterRef = InRangeInter[0];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Interactible"))
                return;
            
            if (other.TryGetComponent<BaseInterBehavior>(out var inter))
                inter.AddSelf(this);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Interactible"))
                return;

            if (!other.TryGetComponent<BaseInterBehavior>(out var inter))
                return;
            
            if (InRangeInter.Contains(inter))
                inter.RemoveSelf();
        }

        #endregion

        
        #region fields

        private static readonly Comparison<BaseInterBehavior> CompareInters = (a, b) =>
            Mathf.RoundToInt(Mathf.Sign(a.DistFromPlayer - b.DistFromPlayer));

        #endregion
    }
}