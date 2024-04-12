using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates
{
    [System.Serializable]
    public struct RecepDatas
    {
        [FieldCompletion(FieldColor.Red, FieldColor.Green)] public ReceptorInter ReceptorInter;

        [FieldColorLerp(FieldColor.Red, FieldColor.Green, 0, 1)] [Range(0, 1)] public float ActivationDelay;

        public RecepDatas(ReceptorInter receptorInter, float activationDelay)
        {
            ReceptorInter = receptorInter;
            ActivationDelay = activationDelay;
        }
    }
}