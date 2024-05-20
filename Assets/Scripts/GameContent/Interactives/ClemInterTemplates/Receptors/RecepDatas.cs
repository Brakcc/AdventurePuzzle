using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates.Receptors
{
    [System.Serializable]
    public struct RecepDatas
    {
        #region properties
        
        [FieldCompletion(FieldColor.Red, FieldColor.Green)] public ReceptorInter ReceptorInter;

        [FieldColorLerp(FieldColor.Red, FieldColor.Green, -1, 1)] [Range(-1, 1)] public float ActivationDelay;

        #endregion
        
        #region constructor
        
        public RecepDatas(ReceptorInter receptorInter, float activationDelay)
        {
            ReceptorInter = receptorInter;
            ActivationDelay = activationDelay;
        }
        
        #endregion
    }
}