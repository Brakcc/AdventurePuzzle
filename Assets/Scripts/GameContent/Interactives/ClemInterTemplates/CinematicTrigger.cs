using GameContent.PlayerScripts;
using GameContent.PlayerScripts.CutScenes;
using UnityEngine;
using Utilities.CustomAttributes;

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
        
        #endregion

        #region fields

        [SerializeField] private CutScene CSRef;

        #endregion
    }
}