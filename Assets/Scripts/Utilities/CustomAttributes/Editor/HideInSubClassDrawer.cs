using UnityEditor;
using UnityEngine;

namespace Utilities.CustomAttributes.Editor
{
    [CustomPropertyDrawer(typeof (HideInSubClass))]
    public class HideInSubClassDrawer : PropertyDrawer {
 
        private static bool ShouldShow(SerializedProperty property) {
            var type = property.serializedObject.targetObject.GetType();
            var field = type.GetField(property.name);
            var declaringType = field.DeclaringType;
            return type == declaringType;
        }
 
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if(ShouldShow(property))
                EditorGUI.PropertyField(position, property); 
            //fun fact: base.OnGUI doesn't work! Check for yourself!
            //Ptn je confirme :D c'est bien de la D
        }
 
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(ShouldShow(property))
                return base.GetPropertyHeight(property, label);
            return 0;
        }
    }
}