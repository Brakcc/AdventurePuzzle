using System;
using UnityEngine;

namespace Utilities.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class InterfaceConstraint : PropertyAttribute
    {
        public readonly Type type;

        public InterfaceConstraint(Type type)
        {
            this.type = type;
        }
    }
}