using System;
using GameContent.PlayerScripts.CutScenes;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    [RequireComponent(typeof(Collider))]
    public class CinematicTrigger : MonoBehaviour
    {
        #region methodes
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            OnStartCinematic();
        }

        private void OnStartCinematic()
        {
            CSRef.OnStartCutScene();
            StartCoroutine(CSRef.HandleCutScene());
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, col.size);
        }

        #endregion

        #region fields
        
        [SerializeField] private CutScene CSRef;

        [SerializeField] private BoxCollider col;

        #endregion
    }
}