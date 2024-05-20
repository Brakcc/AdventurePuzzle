using GameContent.Interactives.ClemInterTemplates.Emitters;
using Utilities.CustomAttributes;

namespace GameContent.Interactives.ClemInterTemplates.Receptors
{
    [System.Serializable]
    public struct NodeDatas
    {
        public DentriteType dendrite;

        [ShowIfTrue("dendrite", new[] { (int)DentriteType.Distributor })]
        public Distributor nodeRef;
        
        [ShowIfTrue("dendrite", new[] { (int)DentriteType.Receptor })]
        public ReceptorInter receptorRef;
    }

    public enum DentriteType
    {
        Distributor,
        Receptor,
        None
    }
}