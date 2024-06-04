using UnityEngine;

namespace GameContent.CameraScripts
{
    public class CameraManager : MonoBehaviour
    {
        #region methodes
        
        private void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position,
                                                    playerRef.position,
                                                    ref _vel,
                                                    followDamping);
        }
        
        #endregion

        #region fields

        [Range(0, 1)]
        [SerializeField] private float followDamping;
        
        [SerializeField] private Transform playerRef;

        private Vector3 _vel;

        #endregion
    }
}


//Lourd ce script t'as vu ?
