using UnityEditor;
using UnityEngine;

namespace Utilities.CustomAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FieldColorLerp))]
    public class FieldColorLerpDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is FieldColorLerp field)
            {
                GUI.color = GetLerpedColor(field.color1, field.color2, property.floatValue, field.minValue, field.maxValue);
                // if (!(property.serializedObject.GetType().IsClass || property.serializedObject.GetType().IsInterface))
                //    GUI.color = GetLerpedColor(field.color1, field.color2, property.floatValue, field.minValue, field.maxValue);
                // else
                //   Debug.LogWarning("field not a number");
            }
                
            else
                Debug.LogWarning("field attribute not found");
            
            EditorGUI.PropertyField(position, property, label);
            GUI.color = Color.white;
        }

        private static Color GetLerpedColor(Color col1, Color col2, float value, float min, float max)
        {
            var tempMax = max - min;
            var tempVal = value - min;
            var t = tempVal / tempMax;

            return Color.Lerp(col1, col2, t);
        }
    }
}