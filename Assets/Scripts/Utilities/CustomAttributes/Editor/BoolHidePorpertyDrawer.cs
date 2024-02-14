using UnityEditor;
using UnityEngine;

//Git
namespace Utilities.CustomAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfBoolTrue))]
    public class BoolHidePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hideIfAttribute = (ShowIfBoolTrue)attribute;
            if (GetConditionalAttributeResult(hideIfAttribute, property))
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }

            return -EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var hideIfAttribute = (ShowIfBoolTrue)attribute;

            if (GetConditionalAttributeResult(hideIfAttribute, property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private static bool GetConditionalAttributeResult(ShowIfBoolTrue attribute, SerializedProperty property)
        {
            var enabled = true;

            var boolPropertyPathArray = property.propertyPath.Split('.');
            boolPropertyPathArray[^1] = attribute.kapaProperty;
            var boolPropertyPath = string.Join(".", boolPropertyPathArray);
            
            var kapaValue = property.serializedObject.FindProperty(boolPropertyPath);

            if (kapaValue != null)
            {
                enabled = kapaValue.boolValue;
            }
            else
            {
                Debug.LogWarning("Conditional Attribute not found");
            }

            return enabled;
        }
    }
}
