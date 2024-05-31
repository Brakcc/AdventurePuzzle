using GameContent.PlayerScripts;
using UnityEngine;

namespace GameContent.CameraScripts
{
    [RequireComponent(typeof(Collider))]
    public class CameraSwitcher : MonoBehaviour
    {
        #region methodes
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            if (!other.TryGetComponent<PlayerStateMachine>(out var p))
                return;

            p.CurrentCameraDatas = camDatas;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            if (!other.TryGetComponent<PlayerStateMachine>(out var p))
                return;

            p.CurrentCameraDatas = new CameraDatas();
        }
        
        #endregion

        #region fields

        [SerializeField] private CameraDatas camDatas;

        #endregion
    }
}