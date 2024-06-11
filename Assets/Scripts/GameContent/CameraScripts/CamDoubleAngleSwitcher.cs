using DG.Tweening;
using GameContent.PlayerScripts;
using UnityEngine;

namespace GameContent.CameraScripts
{
    [RequireComponent(typeof(Collider))]
    public class CamDoubleAngleSwitcher : MonoBehaviour
    {
        #region methodes
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (!other.TryGetComponent<PlayerStateMachine>(out var p))
                return;

            p.TransitionCamDatas.arm.DOMove(p.CamSpecRota[camID].arm.position, 1);
            p.TransitionCamDatas.pivot.DORotate(p.CamSpecRota[camID].pivot.eulerAngles, 0.2f);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (!other.TryGetComponent<PlayerStateMachine>(out var p))
                return;
            
            p.TransitionCamDatas.arm.DOMove(p.InitCamDatas.arm.position, 1);
            p.TransitionCamDatas.pivot.DORotate(p.InitCamDatas.pivot.eulerAngles, 0.2f);
        }
        
        #endregion

        #region fields

        [SerializeField] private int camID;

        #endregion
    }
}