using FMODUnity;
using UnityEngine;

namespace Sounds
{
    public class SoundGroundCheck : MonoBehaviour
    {
        [SerializeField] private EventReference emi;
        private void OnTriggerEnter(Collider ground)
        {
            RuntimeManager.PlayOneShot(emi, transform.position);
        }
    }
}
