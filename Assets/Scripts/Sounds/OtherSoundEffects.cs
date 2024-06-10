using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sounds
{
    public class OtherSoundEffects : MonoBehaviour
    {
        public static OtherSoundEffects OtherSoundEffectInstance;
        [SerializeField] private Transform playerTransform;
        
        [SerializeField] private EventReference cosmosVoice;
        [SerializeField] private EventReference grabSoundEffect;
        [SerializeField] private EventReference energieThrowSound;
        [SerializeField] private EventReference energieGetSound;

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
            RuntimeManager.PlayOneShot(energieThrowSound, playerTransform.position);
        }

        public void PlayEnergGetSound()
        {
            RuntimeManager.PlayOneShot(energieGetSound, playerTransform.position);
        }
    }
}
