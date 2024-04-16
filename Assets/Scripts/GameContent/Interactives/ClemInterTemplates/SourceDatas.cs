namespace GameContent.Interactives.ClemInterTemplates
{
    public struct SourceDatas
    {
        #region properties
        
        public EnergySourceInter Source { get; }

        public EnergyTypes Type { get; }

        #endregion
        
        #region contructor

        public SourceDatas(EnergySourceInter source, EnergyTypes type)
        { 
            Source = source;
            Type = type;
        }
        
        public SourceDatas(EnergySourceInter source = null)
        {
            Source = source;
            Type = source == null ? EnergyTypes.None : source.EnergyType;
        }

        #endregion
    }
}