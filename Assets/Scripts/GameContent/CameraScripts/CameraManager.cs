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
                _isRotating = true;
                _previousAngle = transform.eulerAngles.y;
                _angleOverride = value;
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

            if (!_isRotating)
                return;

            if (Mathf.Abs(_previousAngle - _angleOverride) < 2f)
            {
                _isRotating = false;
                return;
            }

            _previousAngle += Time.deltaTime * Mathf.Sign(_angleOverride);
            transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * Mathf.Sign(_angleOverride));
        }

        #endregion

        #region fields

        [Range(0, 1)]
        [SerializeField] private float followDamping;
        
        [SerializeField] private Transform playerRef;

        private Vector3 _vel;

        private bool _isRotating;

        private float _previousAngle;
        
        private float _angleOverride;

        #endregion
    }
}


//Lourd ce script t'as vu ?
