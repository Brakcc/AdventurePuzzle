using UnityEngine;
using FMODUnity;

namespace GameContent.Narration.Creature
{
    public class SoundTestTest : MonoBehaviour
    {
        [SerializeField] private EventReference emi;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RuntimeManager.PlayOneShot(emi, transform.position);
            }
        }
    }
}