using FMODUnity;
using UnityEngine;

namespace Sounds
{
    public class OtherSoundEffects : MonoBehaviour
    {
        public static OtherSoundEffects OtherSoundEffectInstance;
        [SerializeField] private Transform playerTransform;
        
        [SerializeField] private EventReference cosmosVoice;
        [SerializeField] private EventReference grabSoundEffect;
        [SerializeField] private EventReference energieRecupSound;

        private void Awake()
        {
            OtherSoundEffectInstance = this;
            RuntimeManager.PlayOneShot(cosmosVoice, playerTransform.position);
        }
        
        public void PlayGrabSound()
        {
            RuntimeManager.PlayOneShot(grabSoundEffect, playerTransform.position);
        }

        public void PlayEnergThrowSound()
        {
            RuntimeManager.PlayOneShot(energieRecupSound, playerTransform.position);
        }
    }
}
