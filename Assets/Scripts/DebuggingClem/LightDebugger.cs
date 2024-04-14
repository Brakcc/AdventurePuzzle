using System;
using GameContent.Interactives.ClemInterTemplates;
using UnityEngine;

namespace DebuggingClem
{
    public static class LightDebugger
    {
        public static Color DebugColor(EnergyTypes type) => type switch
        {
            EnergyTypes.None => Color.clear,
            EnergyTypes.Yellow => Color.yellow,
            EnergyTypes.Blue => Color.blue,
            EnergyTypes.Green => Color.green,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "how did u do that wtf ???")
        };
    }
}