using UnityEngine;

namespace GameContent.PlayerScripts.PlayerDatas.PlayerDatasSO
{
    [CreateAssetMenu(fileName = "GroundingDatas", menuName = "PlayerDatasSO/GroundingDatas")]
    public class GroundingDatasSO : AbstractPlayerDatasSO
    {
        public LayerMask groundLayer;

        public float dragSpeed;
    }
}