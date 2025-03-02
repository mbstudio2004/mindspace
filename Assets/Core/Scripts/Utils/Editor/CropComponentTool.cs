using UnityEditor;
using UnityEngine;

namespace Nocci.Utils.Editor
{
    [InitializeOnLoad]
    public static class CropComponentTool
    {
        static CropComponentTool()
        {
            DragAndDrop.AddDropHandler(OnComponentHierarchyDrop);
        }


        private static DragAndDropVisualMode OnComponentHierarchyDrop(int dropTargetInstanceID,
            HierarchyDropFlags dropMode,
            Transform parentForDraggedObjects,
            bool perform)
        {
            var component = GetComponentBeingDragged();
            if (component != null)
            {
                if (perform)
                {
                    var targetGameObject = EditorUtility.InstanceIDToObject(dropTargetInstanceID) as GameObject;

                    if (targetGameObject == null)
                    {
                        return DragAndDropVisualMode.Rejected;
                    }

                    var newComponent = DuplicateComponent(component, targetGameObject);
                    
                    if (newComponent != null)
                    {
                        Undo.DestroyObjectImmediate(component);
                    }
                    
                    Selection.activeGameObject = newComponent.gameObject;
                }

                return DragAndDropVisualMode.Copy;
            }

            return DragAndDropVisualMode.None;
        }

        static Component GetComponentBeingDragged()
        {
            foreach (var draggedObject in DragAndDrop.objectReferences)
            {
                if (draggedObject is Component component)
                {
                    return component;
                }
            }

            return null;
        }

        static Component DuplicateComponent(Component sourceComponent, GameObject targetGameObject)
        {
            var newComponent = Undo.AddComponent(targetGameObject, sourceComponent.GetType());

            var source = new SerializedObject(sourceComponent);
            var dest = new SerializedObject(newComponent);
            var iterator = source.GetIterator();

            while (iterator.NextVisible(true))
            {
                dest.CopyFromSerializedProperty(iterator);
            }

            dest.ApplyModifiedProperties();

            return newComponent;
        }
    }
}