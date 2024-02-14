using System;
using UnityEngine;

//Git
namespace Utilities.CustomAttributes
{
    /// <summary>
    /// <para>/!\ ne fonctionne pas avec d'autre attribut de Unity (SerializeField, Range ...) /!\</para>
    /// <para>c'est rigolo t'as vu ?</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class HideInSubClass : PropertyAttribute
    {
        // tout tient avec ça, y'a quoi maintenant
    }
}