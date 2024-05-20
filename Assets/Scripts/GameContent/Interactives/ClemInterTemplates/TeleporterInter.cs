using GameContent.Interactives.ClemInterTemplates.Receptors;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class TeleporterInter : ReceptorInter
    {
        [SerializeField] private TeleporterInter otherTeleporter;
        
        private bool _canTeleport;
        private bool _justTeleported;

        private bool _teleportStart;
        private Transform _playerToTeleport;
        private void FixedUpdate()
        {
            if (_teleportStart)
            {
                _teleportStart = false;
                _playerToTeleport.position = otherTeleporter.transform.position;
            }
        }
        
        public override void InterAction()
        {
            Debug.Log(CurrentEnergyType);
            _justTeleported = false;
            _canTeleport = (CurrentEnergyType == EnergyTypes.Yellow);
            otherTeleporter._canTeleport = (CurrentEnergyType == EnergyTypes.Yellow);
            GetComponent<CapsuleCollider>().isTrigger = (CurrentEnergyType != EnergyTypes.Blue);
            
            GetComponent<MeshRenderer>().enabled = (CurrentEnergyType != EnergyTypes.Green && CurrentEnergyType != EnergyTypes.None);
            otherTeleporter.GetComponent<MeshRenderer>().enabled = (CurrentEnergyType != EnergyTypes.Green && CurrentEnergyType != EnergyTypes.None);
            
            if (CurrentEnergyType == EnergyTypes.Yellow)
            {
                GetComponent<MeshRenderer>().material.color = new Color32(173,255,47,125);
                otherTeleporter.GetComponent<MeshRenderer>().material.color = new Color32(173,255,47,125);
            }
            else if (CurrentEnergyType == EnergyTypes.Blue)
            {
                GetComponent<MeshRenderer>().material.color = new Color32(65,105,225,255);
                otherTeleporter.GetComponent<MeshRenderer>().material.color = new Color32(65,105,225,255);
            }
            base.InterAction();
        }

        public override void OnReset()
        {
            GetComponent<MeshRenderer>().enabled = false;
            otherTeleporter.GetComponent<MeshRenderer>().enabled = false;
            _canTeleport = false;
            _justTeleported = false;
            
            otherTeleporter._canTeleport = false;
            base.OnReset();
        }

        private void OnTriggerEnter(Collider playerCol)
        {
            if (playerCol.transform.CompareTag("Player") && _canTeleport && !_justTeleported)
            {
                _playerToTeleport = playerCol.transform;
                _teleportStart = true;
                otherTeleporter._justTeleported = true;
            }  
        }
        private void OnTriggerExit(Collider playerCol)
        {
            if (playerCol.transform.CompareTag("Player") && _canTeleport && _justTeleported)
            {
                _justTeleported = false;
            }
        }
    }
}
