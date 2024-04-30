using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.PlayerScripts.PlayerDatas.PlayerDatasSO
{
    [CreateAssetMenu(fileName = "MoveDatas", menuName = "PlayerDatasSO/MoveDatas")]
    public class MoveDatasSO : AbstractPlayerDatasSO
    {
        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, 1, 30)]
        [Range(1, 30)]public float moveSpeed;

        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, 1, 30)]
        [Range(1, 30)]public float holdingRecepMoveSpeed;
        
        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, 1, 15)]
        [Range(1, 15)] public float rotaSpeedCoef;
    }
}