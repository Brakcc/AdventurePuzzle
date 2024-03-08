using System.Collections.Generic;
using GameContent.Interactives;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class InterCheckerState : MonoBehaviour
    {
        #region properties

        public BaseInterBehavior InterRef { get; private set; }

        public List<BaseInterBehavior> InRangeInter { get; set; }

        #endregion

        #region methodes

        private void Update()
        {
            InterRef = InRangeInter?[0];
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