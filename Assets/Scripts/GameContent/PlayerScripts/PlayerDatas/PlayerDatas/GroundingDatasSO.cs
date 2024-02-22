using UnityEngine;

namespace GameContent.PlayerScripts.PlayerDatas.PlayerDatas
{
    [CreateAssetMenu(fileName = "GroundingDatas", menuName = "PlayerDatas/GroundingDatas")]
    public class GroundingDatasSO : AbstractPlayerDatasSO
    {
        public LayerMask groundLayer;

        public float dragSpeed;
    }
}