using System;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public readonly struct SourceDatas
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

        public static Color GetTypedColor(EnergyTypes type) => type switch
        {
            EnergyTypes.None => new Color(0, 0, 0, 0),
            EnergyTypes.Yellow => Color.yellow,
            EnergyTypes.Green => Color.green,
            EnergyTypes.Blue => Color.blue,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "allez zebi")
        };
            
        
        #endregion
    }
}