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
        
        #region fields
        
        [SerializeField] [Range(1, 3)] private short connectionID;
        
        public DentriteType dendrite;
        
        [ShowIfTrue("dendrite", new[] { (int)DentriteType.Distributor })]
        public Distributor nodeRef;
        
        [ShowIfTrue("dendrite", new[] { (int)DentriteType.Receptor })]
        public ReceptorInter receptorRef;
        
        #endregion
    }

    public enum DentriteType
    {
        Distributor,
        Receptor,
        None
    }
}