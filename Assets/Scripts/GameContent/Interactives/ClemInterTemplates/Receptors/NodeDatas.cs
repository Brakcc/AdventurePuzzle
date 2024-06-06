using GameContent.Interactives.ClemInterTemplates.Emitters;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.Interactives.ClemInterTemplates.Receptors
{
    [System.Serializable]
    public struct NodeDatas
    {
        #region properties

        public short ConnectionID => (short)(connectionID - 1);

        #endregion

        #region methodes

        public void SetProperties(MaterialPropertyBlock mB)
        {
            foreach (var c in cableRends)
            {
                c.SetPropertyBlock(mB);
            }
        }

        #endregion
        
        #region fields
        
        [SerializeField] [Range(2, 4)] private short connectionID;
        
        public DentriteType dendrite;
        
        [ShowIfTrue("dendrite", new[] { (int)DentriteType.Distributor })]
        [FieldCompletion] public Distributor distributorRef;
        
        [ShowIfTrue("dendrite", new[] { (int)DentriteType.Receptor })]
        [FieldCompletion] public ReceptorInter receptorRef;

        public Renderer[] cableRends;

        #endregion
    }

    public enum DentriteType
    {
        Receptor,
        Distributor,
        None
    }
}