namespace GameContent.Interactives.ClemInterTemplates
{
    public struct SourceDatas
    {
        public EnergySourceInter Source { get; }

        public EnergyTypes Type { get; }

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
    }
}