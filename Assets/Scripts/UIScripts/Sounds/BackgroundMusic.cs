using System.Collections;
using FMODUnity;
using UnityEngine;

namespace UIScripts.Sounds
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private PlaySound atterrissageVaisseau;
        [SerializeField] private PlaySound decolageVaisseau;
        [SerializeField] PlaySound forestBackgroundMusic;
        
        
        private void Start()
        {
            StartCoroutine(PlayBackgroundMusic());
            atterrissageVaisseau.PlayMySound();
        }

        IEnumerator PlayBackgroundMusic()
        {
            yield return new WaitForSeconds(1f);
            forestBackgroundMusic.PlayMySound();
            forestBackgroundMusic.GetComponent<StudioEventEmitter>().EventDescription.getLength(out var time);
            yield return new WaitForSeconds(time - 1f);
        }

        public void PlayDecollage()
        {
            decolageVaisseau.PlayMySound();
        }
    }
}
