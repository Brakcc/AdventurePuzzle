using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using GameContent.CameraScripts;

namespace Sounds
{
    public class PlayerFallSound : MonoBehaviour
    {
        private List<Collider> _collidersTouched = new List<Collider>(); 
        [SerializeField] private Collider playerCollidToIgnore1;
        [SerializeField] private Collider playerCollidToIgnore2;
        
        [SerializeField] private EventReference sound;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other != playerCollidToIgnore1 && other != playerCollidToIgnore2 && !other.GetComponent<CamAngleOverrider>())
            {
                if (_collidersTouched.Count == 0)
                {
                    RuntimeManager.PlayOneShot(sound, transform.position);
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
    }
}
