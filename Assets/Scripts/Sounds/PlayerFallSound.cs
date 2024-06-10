using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using GameContent.CameraScripts;
using GameContent.Interactives.ClemInterTemplates.Receptors;

namespace Sounds
{
    public class PlayerFallSound : MonoBehaviour
    {
        private List<Collider> _collidersTouched = new List<Collider>(); 
        [SerializeField] private Collider playerCollidToIgnore1;
        [SerializeField] private Collider playerCollidToIgnore2;
        
        [SerializeField] private EventReference sound;
        [SerializeField] private float valueSound;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other != playerCollidToIgnore1 
                && other != playerCollidToIgnore2 
                && !other.GetComponent<CamAngleOverrider>() 
                && !other.GetComponent<TeleporterRecep>()
                && !other.GetComponent<WalkSoundPlayer>())
            {
                if (_collidersTouched.Count == 0)
                {
                   PlayFallInstance();
                }
                _collidersTouched.Add(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != playerCollidToIgnore1 && other != playerCollidToIgnore2)
            {
                _collidersTouched.Remove(other);
            }
        }
        
        void PlayFallInstance()
        {
            EventInstance instanceStep = RuntimeManager.CreateInstance(sound);
            
            instanceStep.getDescription(out EventDescription stepDescription);
            stepDescription.getParameterDescriptionByName("CosmosTerrain",
                out PARAMETER_DESCRIPTION stepParameterDescription);
            var idCosmosTerrain = stepParameterDescription.id;

            instanceStep.setParameterByID(idCosmosTerrain, valueSound);
            
            instanceStep.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));

            instanceStep.start();
        }
    }
}
