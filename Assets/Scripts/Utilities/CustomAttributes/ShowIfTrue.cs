using System;
using UnityEngine;

//Git
namespace Utilities.CustomAttributes
{
    /// <summary>
    /// <para>montre un field s.s.i un autre est au bon enum/int </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ShowIfTrue : PropertyAttribute
    {
        public readonly string kapaProperty;
        public readonly int[] kapaFs;
        
        public ShowIfTrue(string kapaProperty, int[] kapas)
        {
            this.kapaProperty = kapaProperty;
            kapaFs = kapas;
        }
    }
}