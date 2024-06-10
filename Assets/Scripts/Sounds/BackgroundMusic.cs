using System.Collections;
using FMODUnity;
using UnityEngine;

namespace Sounds
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private EventReference atterrissageVaisseau;
        [SerializeField] private EventReference decolageVaisseau;
        [SerializeField] private EventReference forestBackgroundMusic;
        [SerializeField] private Transform thePlayer;
        
        
        private void Start()
        {
            StartCoroutine(PlayBackgroundMusic());
            RuntimeManager.PlayOneShot(atterrissageVaisseau, thePlayer.position);
        }

        IEnumerator PlayBackgroundMusic()
        {
            yield return new WaitForSeconds(1f);
            RuntimeManager.PlayOneShot(forestBackgroundMusic, thePlayer.position);
            yield return new WaitForSeconds(99);
        }

        public void PlayDecollage()
        {
            RuntimeManager.PlayOneShot(decolageVaisseau, thePlayer.position);
        }
    }
}
