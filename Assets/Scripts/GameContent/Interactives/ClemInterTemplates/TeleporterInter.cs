using GameContent.Interactives.ClemInterTemplates.Receptors;
using UIScripts.Sounds;
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
        
        //For TeleAltPush
        [HideInInspector] public bool blue;

        private void Start()
        {
            InterAction();
        }

        private void FixedUpdate()
        {
            if (_teleportStart)
            {
                _teleportStart = false;
                _playerToTeleport.position = otherTeleporter.transform.position;
            }
            base.OnFixedUpdate();
        }
        
        public override void InterAction()
        {
            base.InterAction();
            
            _justTeleported = false;
            _canTeleport = (CurrentEnergyType == EnergyTypes.None);
            otherTeleporter._canTeleport = (CurrentEnergyType == EnergyTypes.None);
            
            GetComponent<Collider>().isTrigger = (CurrentEnergyType != EnergyTypes.Blue);
            otherTeleporter.GetComponent<Collider>().isTrigger = (CurrentEnergyType != EnergyTypes.Blue);
            
            if (CurrentEnergyType == EnergyTypes.Blue)
            {
                GetComponent<MeshRenderer>().material.color = new Color32(65,105,225,255);
                otherTeleporter.GetComponent<MeshRenderer>().material.color = new Color32(65,105,225,255);
                IsMovable = otherTeleporter.IsMovable = true;
            }
            else
            {
                GetComponent<MeshRenderer>().material.color = new Color32(80,255,47,125);
                otherTeleporter.GetComponent<MeshRenderer>().material.color = new Color32(80,255,47,125);
            }
            blue = otherTeleporter.blue = (CurrentEnergyType == EnergyTypes.Blue);
        }

        public override void OnReset()
        {
            _canTeleport = true;
            otherTeleporter._canTeleport = true;
            _justTeleported = false;
            GetComponent<Collider>().isTrigger = true;
            otherTeleporter.GetComponent<Collider>().isTrigger = true;
            
            base.OnReset();
        }

        private void OnTriggerEnter(Collider playerCol)
        {
            if (playerCol.transform.CompareTag("Player") && _canTeleport && !_justTeleported)
            {
                GetComponent<PlaySound>().PlayMySound();
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
