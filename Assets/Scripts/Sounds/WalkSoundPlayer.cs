using System.Collections;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

namespace Sounds
{
    public class WalkSoundPlayer : MonoBehaviour
    {
        [SerializeField] private EventReference sound;
        [SerializeField] private Transform walkVelocity;
        private Vector3 _pos;
        private bool _canPlay;

        [SerializeField] private float valueSound;

        [SerializeField] private int waterLayer;
        [SerializeField] private int stoneLayer;
        [SerializeField] private int metalLayer;
        
        private void Start()
        {
            _canPlay = true;
        }

        void FixedUpdate()
        {
            if ((walkVelocity.position.x != _pos.x || walkVelocity.position.z != _pos.z) && walkVelocity.position.y == _pos.y && _canPlay)
            {
                _canPlay = false;
                PlayStepInstance();
                StartCoroutine(WaitForNextSound());
            }

            _pos = walkVelocity.position;
        }

        private IEnumerator WaitForNextSound()
        {
            yield return new WaitForSecondsRealtime(0.3f);
            _canPlay = true;
        }

        void PlayStepInstance()
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

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == waterLayer)
            {
                valueSound = 0;
            }
            /*else if (other.CompareTag())
            {
                valueSound = 2;
            }*/
            else if (other.gameObject.layer == stoneLayer)
            {
                //Stone
                valueSound = 4;
            }
            else if (other.gameObject.layer == metalLayer)
            {
                //Metal
                valueSound = 3;
            }
            else
            {
                valueSound = 1;
            }
        }
    }
}
