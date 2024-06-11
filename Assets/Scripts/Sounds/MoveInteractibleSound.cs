using System.Collections;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

namespace Sounds
{
    public class MoveInteractibleSound : MonoBehaviour
    {
        [SerializeField] private EventReference sound;
        [SerializeField] private Transform elementVelocity;
        private Vector3 _pos;
        private bool _canPlay;

        [SerializeField] private float valueSound;
        
        private void Start()
        {
            _canPlay = true;
        }

        void FixedUpdate()
        {
            if ((elementVelocity.position.x != _pos.x || elementVelocity.position.z != _pos.z) && elementVelocity.position.y == _pos.y && _canPlay)
            {
                _canPlay = false;
                PlayStepInstance();
                StartCoroutine(WaitForNextSound());
            }

            _pos = elementVelocity.position;
        }

        private IEnumerator WaitForNextSound()
        {
            yield return new WaitForSecondsRealtime(0.25f);
            _canPlay = true;
        }

        void PlayStepInstance()
        {
            EventInstance instanceStep = RuntimeManager.CreateInstance(sound);
            
            instanceStep.getDescription(out EventDescription stepDescription);
            stepDescription.getParameterDescriptionByName("ÉlémentTerrain",
                out PARAMETER_DESCRIPTION stepParameterDescription);
            var idCosmosTerrain = stepParameterDescription.id;

            instanceStep.setParameterByID(idCosmosTerrain, valueSound);
            
            instanceStep.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));

            instanceStep.start();
        }

        private void OnTriggerStay(Collider other)
        {
            
            if (other.gameObject.layer == 4)
            {
                valueSound = 0;
            }
            /*else if (other.CompareTag())
            {
                valueSound = 2;
            }*/
            else if (other.gameObject.layer == 4)
            {
                valueSound = 3;
            }
            else if (other.gameObject.layer == 18)
            {
                valueSound = 4;
            }
            else
            {
                valueSound = 1;
            }
            
        }
    }
}
