using System;
using UnityEngine;
using Utilities.CustomAttributes.FieldColors;

//Git
namespace Utilities.CustomAttributes
{
    /// <summary>
    /// <para>rappel de completion de field</para>
    /// <para>prevention de nullRefs</para>
    /// <remarks>Placer en 1er attribute pour fonctionner</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class FieldCompletion : PropertyAttribute
    {
        public Color unCheckedColor;
        public Color checkedColor;

        public FieldCompletion(FieldColor _uncheckedColor = FieldColor.Red, FieldColor _checkedColor = FieldColor.Cyan)
        {
            unCheckedColor = GetFieldColor.GetFC(_uncheckedColor);

            checkedColor = GetFieldColor.GetFC(_checkedColor);
        }

        
    }
}