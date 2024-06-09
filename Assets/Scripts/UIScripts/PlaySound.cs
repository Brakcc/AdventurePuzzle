using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace UIScripts
{
    public class PlaySound : MonoBehaviour
    {
        [Header("Nécessite un FPro Sound Emitter")]
        public bool isAmbiance;

        private void Start()
        {
            GetComponent<StudioEventEmitter>().EventInstance.stop(STOP_MODE.IMMEDIATE);
        }

        public void PlayMySound()
        {
            SoundManager.SoundInstance.PlayEventSound(GetComponent<StudioEventEmitter>(), isAmbiance);
        }

    }
}
