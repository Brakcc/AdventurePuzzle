using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.PlayerScripts.PlayerDatas.PlayerDatasSO
{
    [CreateAssetMenu(fileName = "MoveDatas", menuName = "PlayerDatasSO/MoveDatas")]
    public class MoveDatasSO : AbstractPlayerDatasSO
    {
        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, 1, 30)]//git
        [Range(1, 30)]public float moveSpeed;//GIT
//GIT
        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, 1, 30)]//GIT
        [Range(1, 30)]public float holdingRecepMoveSpeed;//GIT
        //GIT
        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, 1, 15)]//GIT
        [Range(1, 15)] public float rotaSpeedCoef;//GIT
    }
}