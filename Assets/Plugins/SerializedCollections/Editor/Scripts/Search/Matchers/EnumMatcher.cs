using System;
using Plugins.SerializedCollections.Editor.Scripts.Utility;
using UnityEditor;

namespace Plugins.SerializedCollections.Editor.Scripts.Search.Matchers
{
    public class EnumMatcher : Matcher
    {
        public override bool IsMatch(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.Enum && SCEditorUtility.TryGetTypeFromProperty(property, out var type))
            {
                foreach (var text in SCEnumUtility.GetEnumCache(type).GetNamesForValue(property.enumValueFlag))
                {
                    if (text.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }
    }
}