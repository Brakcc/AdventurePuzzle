using System.Collections;
using UnityEngine;

namespace Plugins.SerializedCollections.Editor.Scripts.KeyListGenerators
{
    public abstract class KeyListGenerator : ScriptableObject
    {
        public abstract IEnumerable GetKeys(System.Type type);
    }
}