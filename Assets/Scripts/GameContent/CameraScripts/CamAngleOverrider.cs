using GameContent.PlayerScripts;
using UnityEngine;

namespace GameContent.CameraScripts
{
    [RequireComponent(typeof(Collider))]
    public class CamAngleOverrider : MonoBehaviour
    {
        #region methodes
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (!other.TryGetComponent<PlayerStateMachine>(out var p))
                return;

            p.InitCamManager.AngleOverride = newAngle;
            p.TransCamManager.AngleOverride = newAngle;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (!other.TryGetComponent<PlayerStateMachine>(out var p))
                return;

            p.InitCamManager.AngleOverride = PlayerStateMachine.InitCamAngle;
            p.TransCamManager.AngleOverride = PlayerStateMachine.InitCamAngle;
        }
        
        #endregion

        #region fields

        [SerializeField] private float newAngle;

        #endregion
    }
}