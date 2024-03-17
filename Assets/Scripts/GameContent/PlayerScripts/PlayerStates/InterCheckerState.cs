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
            if (InRangeInter.Count <= 0)
                return;
            
            InterRef = InRangeInter[0];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Interactible"))
                return;
            
            if (other.TryGetComponent<BaseInterBehavior>(out var inter))
                inter.AddSelf(this);
        }

        #endregion
    }
}