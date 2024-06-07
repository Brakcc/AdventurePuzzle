using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.CameraScripts
{
    [System.Serializable]
    public struct CameraDatas
    {
        [FieldCompletion] public Transform pivot;
        [FieldCompletion] public Transform arm;

        public CameraDatas(Transform pivot = default, Transform arm = default)
        {
            this.pivot = pivot;
            this.arm = arm;
        }
    }
}