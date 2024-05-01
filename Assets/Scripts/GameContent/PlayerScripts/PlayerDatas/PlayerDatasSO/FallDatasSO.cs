using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.PlayerScripts.PlayerDatas.PlayerDatasSO
{
    [CreateAssetMenu(fileName = "FallDatas", menuName = "PlayerDatasSO/FallDatas")]
    public class FallDatasSO : AbstractPlayerDatasSO
    {
        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, 1, 15)]
        [Range(0, 30)] public float fallSpeed;
    }
}