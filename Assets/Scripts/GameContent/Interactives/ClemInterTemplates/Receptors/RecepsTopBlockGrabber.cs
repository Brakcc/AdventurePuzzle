using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Receptors
{
    [RequireComponent(typeof(Collider))]
    public class RecepsTopBlockGrabber : MonoBehaviour
    {
        #region properties

        public List<ReceptorInter> RecepRefs { get; private set; }

        #endregion

        #region methodes

        private void Start()
        {
            RecepRefs = new List<ReceptorInter>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Interactible"))
                return;
            
            if (!other.TryGetComponent<ReceptorInter>(out var recep))
                return;
            
            RecepRefs.Add(recep);
            recep.SetRBConstraints((RigidbodyConstraints)112);
            Debug.Log($"{recep.name}  added");
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Interactible"))
                return;
            
            if (!other.TryGetComponent<ReceptorInter>(out var recep))
                return;
            
            if (!RecepRefs.Contains(recep))
                return;
            
            recep.SetRBConstraints((RigidbodyConstraints)Constants.BitFlagRBConstraintRotaPlan);
            RecepRefs.Remove(recep);
            Debug.Log($"{recep.name}  removed");
        }

        #endregion
    }
}