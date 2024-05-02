using System.Collections;
using UnityEngine;
using UnityEditor;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class TeleporterInter : ReceptorInter
    {
        public TeleporterInter otherTeleporter;
        
        public bool _justTeleported;
        public bool _canTeleport;
        
        public override void InterAction()
        {
            GetComponent<MeshRenderer>().enabled = true;
            otherTeleporter.GetComponent<MeshRenderer>().enabled = true;
            _justTeleported = false;
            _canTeleport = true;
            
            otherTeleporter._justTeleported = false;
            otherTeleporter._canTeleport = true;
        }

        public override void PlayerCancel()
        {
            GetComponent<MeshCollider>().enabled = false;
            otherTeleporter.GetComponent<MeshCollider>().enabled = false;
            _canTeleport = false;
            _justTeleported = false;
            
            otherTeleporter._justTeleported = false;
            otherTeleporter._canTeleport = false;
        }

        private void OnTriggerEnter(Collider playerCol)
        {
            if (playerCol.transform.CompareTag("Player") && _canTeleport)
            {
                Debug.Log("screams of pain");
                _justTeleported = true;
                if (!otherTeleporter._justTeleported)
                {
                    playerCol.transform.position = otherTeleporter.transform.position;
                }
            }  
        }
        /*private void OnTriggerExit(Collider playerCol)
        {
            if (playerCol.transform.CompareTag("Player") && _canTeleport && _justTeleported)
            {
                Debug.Log(gameObject.name);
                _justTeleported = false;
                otherTeleporter._justTeleported = false;
            }
        }*/
    }
}
