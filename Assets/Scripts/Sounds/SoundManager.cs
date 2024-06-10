using UnityEngine;
using FMODUnity;

namespace UIScripts
{
    public class SoundManager : MonoBehaviour
    {
        static public SoundManager SoundInstance;
        [HideInInspector] public float volume;
        [HideInInspector] public float musicVolume;
        [HideInInspector] public float soundEffectVolume;
        

        private void Awake()
        {
            SoundInstance = this;
        }

        public void PlayEventSound(StudioEventEmitter myEmitter, bool isAmbiance)
        {
            myEmitter.EventInstance.setVolume(volume);
            myEmitter.EventInstance.start();
        }
        
        public void SetUpVolumes(float volumeValue, float musiqueValue, float soundEffectValue)
        {
            SoundInstance.volume = volumeValue / 100;
            SoundInstance.musicVolume = SoundInstance.volume / 100 * musiqueValue;
            SoundInstance.soundEffectVolume = SoundInstance.volume / 100 * soundEffectValue;
        }
    }
}
