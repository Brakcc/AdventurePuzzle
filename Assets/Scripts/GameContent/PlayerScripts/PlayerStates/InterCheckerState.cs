using System;
using System.Collections.Generic;
using GameContent.Interactives;
using GameContent.Interactives.ClemInterTemplates;
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
                    if (InterRef is not null)
                        InterRef = null;
                    return;
                case >= 2:
                    InRangeInter.Sort(CompareIntersByParams);
                    break;
            }

            InterRef = InRangeInter[0];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Interactible"))
                return;
            
            if (other.TryGetComponent<BaseInterBehavior> (out var inter))
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

        private static readonly Comparison<BaseInterBehavior> CompareIntersByDist = (a, b) =>
            Mathf.RoundToInt(Mathf.Sign(a.DistFromPlayer - b.DistFromPlayer));
        
        private static readonly Comparison<BaseInterBehavior> CompareIntersByAngle = (a, b) =>
            Mathf.RoundToInt(Mathf.Sign(a.AngleWithPlayer - b.AngleWithPlayer));

        private static readonly Comparison<BaseInterBehavior> CompareIntersByParams = (a, b) =>
            Mathf.RoundToInt(Mathf.Sign(a.DistFromPlayer - b.DistFromPlayer)) / 2 *
            (Mathf.RoundToInt(Mathf.Sign(a.AngleWithPlayer - b.AngleWithPlayer)) / 2);

        #endregion
    }
}