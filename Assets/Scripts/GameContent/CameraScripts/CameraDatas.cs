using UnityEngine;

namespace GameContent.CameraScripts
{
    [System.Serializable]
    public struct CameraDatas
    {
        public Vector3 pivot;
        public Vector3 arm;

        public CameraDatas(Vector3 pivot = default, Vector3 arm = default)
        {
            this.pivot = pivot;
            this.arm = arm;
        }
    }
}