using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.PlayerScripts.PlayerDatas.PlayerDatasSO
{
    [CreateAssetMenu(fileName = "CollisionsDatas", menuName = "PlayerDatasSO/CollisionsDatas")]
    public class CollisionDatasSO : AbstractPlayerDatasSO
    {
        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, 0.01f, 0.2f)]
        [Range(0.01f, 0.2f)] public float widthCorrector;
        
        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, -1, 1)]
        [Range(-1, 1)] public float midHeightCorrector;
        
        public LayerMask blockMask;
    }
}