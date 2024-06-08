using FMODUnity;
using UnityEngine;

namespace UIScripts
{
    public class PlaySound : MonoBehaviour
    {
        [Header("Nécessite un FPro Sound Emitter")]
        public bool isAmbiance;
        
        public StudioEventEmitter MyEmitter;
        [HideInInspector] public float volume;

        public void PlayEventSound()
        {
            MyEmitter.EventInstance.setVolume(volume);
            MyEmitter.EventInstance.start();
        }

    }
}
