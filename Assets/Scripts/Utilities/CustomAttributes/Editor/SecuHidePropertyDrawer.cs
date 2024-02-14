using UnityEditor;
using UnityEngine;

//Git
namespace Utilities.CustomAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfSecu))]
    public class SecuHidePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hideIfAttribute = (ShowIfSecu)attribute;
            if (GetConditionalAttributeResult(hideIfAttribute, property))
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }

            return -EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var hideIfAttribute = (ShowIfSecu)attribute;

            if (GetConditionalAttributeResult(hideIfAttribute, property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private static bool GetConditionalAttributeResult(ShowIfSecu attribute, SerializedProperty property)
        {
            var enabled = true;

            var boolPropertyPathArray = property.propertyPath.Split('.');
            boolPropertyPathArray[^1] = attribute.kapaProperSecurity;
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