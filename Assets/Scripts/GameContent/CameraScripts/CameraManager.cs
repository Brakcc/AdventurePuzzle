using DG.Tweening;
using UnityEngine;

namespace GameContent.CameraScripts
{
    public class CameraManager : MonoBehaviour
    {
        #region properties

        public float AngleOverride
        {
            set
            {
                _angleOverride = value;
                transform.DORotate(new Vector3(0, _angleOverride, 0), 2f);
            }
        }

        #endregion
        
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
        
        private float _angleOverride;

        #endregion
    }
}


//Lourd ce script t'as vu ?
