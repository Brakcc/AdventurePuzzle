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
            GetComponent<MeshRenderer>().enabled = true;
            otherTeleporter.GetComponent<MeshRenderer>().enabled = true;
            _canTeleport = true;
            _justTeleported = false;
            
            otherTeleporter._canTeleport = true;
        }

        public override void OnReset()
        {
            GetComponent<MeshRenderer>().enabled = false;
            otherTeleporter.GetComponent<MeshRenderer>().enabled = false;
            _canTeleport = false;
            _justTeleported = false;
            
            otherTeleporter._canTeleport = false;
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
