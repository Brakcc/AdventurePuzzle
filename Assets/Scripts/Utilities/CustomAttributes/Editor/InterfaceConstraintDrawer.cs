using UnityEditor;
using UnityEngine;

namespace Utilities.CustomAttributes.Editor
{
    [CustomPropertyDrawer(typeof(InterfaceConstraint))]
    public class InterfaceConstraintDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ObjectReference)
        {
            Debug.LogWarning($"{property.serializedObject.targetObject.GetType()} - {property.displayName}: This drawer must be used only on Object types");
            return;
        }

        var constraint = attribute as InterfaceConstraint;

        var evt = Event.current;
        
        if (DragAndDrop.objectReferences.Length > 0 && position.Contains(evt.mousePosition))
        {
            var draggedObject = DragAndDrop.objectReferences[0] as GameObject;

            if (constraint != null && (draggedObject == null || (draggedObject != null && draggedObject.GetComponent(constraint.type) == null)))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;

                if (evt.type == EventType.DragExited)
                    Debug.LogWarning($"Object assigned to '{property.name}' must implement interface '{constraint.type}'");
            }
        }

        property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(GameObject), true);

        if (property.objectReferenceValue == null)
            return;
        
        var tempGo = property.objectReferenceValue as Component;
        
        if (constraint == null || tempGo == null || tempGo.GetComponent(constraint.type) != null)
            return;
            
        property.objectReferenceValue = null;
        Debug.LogWarning($"Object assigned to '{property.name}' must implement interface '{constraint.type}'");
    }
    }
}