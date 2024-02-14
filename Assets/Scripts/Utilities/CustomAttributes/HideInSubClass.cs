using System;
using UnityEngine;

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
        // c'est moi, y'a quoi maintenant
    }
}