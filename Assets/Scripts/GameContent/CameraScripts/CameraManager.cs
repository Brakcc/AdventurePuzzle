using UnityEngine;

namespace GameContent.CameraScripts
{
    public class CameraManager : MonoBehaviour
    {
        #region methodes
        
        private void Update()
        {
            transform.position = playerRef.position;
        }
        
        #endregion

        #region fields

        [SerializeField] private Transform playerRef;

        #endregion
    }
}
