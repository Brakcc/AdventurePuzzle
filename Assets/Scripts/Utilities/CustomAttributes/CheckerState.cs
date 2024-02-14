using System;
using UnityEngine;
using Utilities.CustomAttributes.FieldColors;

namespace Utilities.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                 AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class CheckerState : PropertyAttribute
    {
        public Color unCheckedColor;
        public Color checkedColor;

        public CheckerState(FieldColor unCheckedColor = FieldColor.Red, FieldColor checkedColor = FieldColor.Green)
        {
            this.unCheckedColor = GetFieldColor.GetFC(unCheckedColor);
            this.checkedColor = GetFieldColor.GetFC(checkedColor);
        }
    }
}