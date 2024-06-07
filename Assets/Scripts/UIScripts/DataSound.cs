using UnityEngine;

namespace UIScripts
{
    public class DataSound : MonoBehaviour
    {
        public float generalVolume;
        public float musicVolume;
        public float soundEffectsVolume;

        public DataSound(float newMainVolume, float newMusicVolume, float newSoundEffectVolume)
        {
            generalVolume = newMainVolume;
            musicVolume = newMusicVolume;
            soundEffectsVolume = newSoundEffectVolume;
        }
    }
}
