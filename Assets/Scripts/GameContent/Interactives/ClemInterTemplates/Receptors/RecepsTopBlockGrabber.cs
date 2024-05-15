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
            recep.IsOnTop = true;
            recep.SetRBConstraints((RigidbodyConstraints)112);
            //Debug.Log($"{recep.name}  added");
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
            recep.IsOnTop = false;
            RecepRefs.Remove(recep);
            //Debug.Log($"{recep.name}  removed");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawWireCube(col.transform.position, col.bounds.size);
        }

        #endregion

        #region fields

        [SerializeField] private Collider col;

        #endregion
    }
}