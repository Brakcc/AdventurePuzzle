using FMODUnity;
using GameContent.Interactives.ClemInterTemplates;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using UnityEngine;

namespace Sounds
{
    public class SoundGroundCheck : MonoBehaviour
    {
        [SerializeField] private EventReference emi;
        private void OnTriggerEnter(Collider ground)
        {
            if (transform.parent.GetComponent<TeleporterRecep>() == null || (transform.parent.GetComponent<TeleporterRecep>().CurrentEnergyType != EnergyTypes.Green))
            {
                RuntimeManager.PlayOneShot(emi, transform.position);
            }
        }
    }
}
