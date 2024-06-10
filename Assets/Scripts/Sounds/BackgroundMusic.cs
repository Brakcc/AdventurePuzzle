using System.Collections;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Sounds
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private EventReference atterrissageVaisseau;
        [SerializeField] private EventReference decolageVaisseau;
        [SerializeField] private EventReference backgroundMusic;
        [SerializeField] private Transform thePlayer;
        private EventInstance _instanceAmbience;
        
        
        private void Start()
        {
            StartCoroutine(PlayBackgroundMusic());
            RuntimeManager.PlayOneShot(atterrissageVaisseau, thePlayer.position);
        }

        IEnumerator PlayBackgroundMusic()
        {
            yield return new WaitForSeconds(1f);
            
            _instanceAmbience = RuntimeManager.CreateInstance(backgroundMusic);
            _instanceAmbience.set3DAttributes(RuntimeUtils.To3DAttributes(thePlayer.transform));
            _instanceAmbience.start();
            
            yield return new WaitForSeconds(99);
            PlayBackgroundMusic(); //? Maybe
        }

        public void PlayDecollage()
        {
            RuntimeManager.PlayOneShot(decolageVaisseau, thePlayer.position);
        }

        public void ChangeBackgroundMusic(EventReference newBackgroundMusic)
        {
            _instanceAmbience.stop(STOP_MODE.ALLOWFADEOUT);
            StopCoroutine(PlayBackgroundMusic());
            backgroundMusic = newBackgroundMusic;
            StartCoroutine(PlayBackgroundMusic());
        }
    }
}
