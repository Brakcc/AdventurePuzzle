using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.PlayerScripts.PlayerDatas.PlayerDatasSO
{
    [CreateAssetMenu(fileName = "JumpDatas", menuName = "PlayerDatasSO/JumpDatas")]
    public class JumpDatasSO : AbstractPlayerDatasSO
    {
        public float jumpForce;

        public float coyoteTime;

        public float jumpBuffer;

        [FieldColorLerp(0, 1)] [Range(0, 1)] public float airControlCoef;
    }
}