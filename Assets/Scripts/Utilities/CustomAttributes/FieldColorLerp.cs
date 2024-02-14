using System;
using UnityEngine;
using Utilities.CustomAttributes.FieldColors;

//Git
namespace Utilities.CustomAttributes
{
    /// <summary>
    /// <para>UTILISER SUR DES VALEURS NUMERIQUES</para>
    /// <para>la secu pour eviter d'ajouter cet attribut sur auter chose est en cours</para>
    /// <remarks>Placer en 1er attribute pour fonctionner</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class FieldColorLerp : PropertyAttribute
    {
        public Color color1;
        public Color color2;
        public readonly float maxValue;
        public readonly float minValue;

        public FieldColorLerp(FieldColor _color1 = FieldColor.Red, FieldColor _color2 = FieldColor.Green,
            float _maxValue = float.MaxValue, float _minValue = float.MinValue)
        {
            color1 = GetFieldColor.GetFC(_color1);
            color2 = GetFieldColor.GetFC(_color2);
            maxValue = _maxValue;
            minValue = _minValue;
        }
        public FieldColorLerp(float _maxValue = float.MaxValue, float _minValue = float.MinValue)
        {
            color1 = GetFieldColor.GetFC(FieldColor.Green);
            color2 = GetFieldColor.GetFC(FieldColor.Red);
            maxValue = _maxValue;
            minValue = _minValue;
        }
    }
}