using System;
using UnityEngine;

namespace Utilities.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ShowIfBoolTrue : PropertyAttribute
    {
        public readonly string kapaProperty;
        
        public ShowIfBoolTrue(string kapaProperty)
        {
            this.kapaProperty = kapaProperty;
        }
    }
}