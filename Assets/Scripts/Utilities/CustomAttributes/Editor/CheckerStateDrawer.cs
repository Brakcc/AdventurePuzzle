using UnityEditor;
using UnityEngine;

namespace Utilities.CustomAttributes.Editor
{
    [CustomPropertyDrawer(typeof(CheckerState))]
    public class CheckerStateDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var field = attribute as CheckerState;

            if (property.boolValue)
            {
                if (field != null)
                    GUI.color = field.checkedColor;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = Color.white;
            }
            else
            {
                if (field != null)
                    GUI.color = field.unCheckedColor;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = Color.white;
            }
        }
    }
}