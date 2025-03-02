using UnityEditor;
using UnityEngine;

namespace Nocci.Utils.Editor
{
    [InitializeOnLoad]
    public static class HierarchyScriptDropHandler
    {
        static HierarchyScriptDropHandler()
        {
            DragAndDrop.AddDropHandler(OnScriptHierarchyDrop);
        }

        private static DragAndDropVisualMode OnScriptHierarchyDrop(int dragInstanceId,HierarchyDropFlags dropMode, Transform parentForDraggedObjects, bool perform)
        {
            var monoScript = GetScriptBeingDragged();
            if (monoScript != null)
            {
                if (perform)
                {
                    var gameObject = CreateAndRename(monoScript.name);
                    var component = gameObject.AddComponent(monoScript.GetClass());
                }

                return DragAndDropVisualMode.Copy;
            }

            return DragAndDropVisualMode.None;
        }

        static MonoScript GetScriptBeingDragged()
        {
            foreach (var draggedObject in DragAndDrop.objectReferences)
            {
                if (draggedObject is MonoScript monoScript)
                {
                    System.Type scriptType = monoScript.GetClass();
                    if (scriptType != null && scriptType.IsSubclassOf(typeof(MonoBehaviour)))
                        return monoScript;
                }
            }

            return null;
        }

        static GameObject CreateAndRename(string startingName)
        {
            var gameObject = new GameObject(startingName);

            if (Selection.activeGameObject)
            {
                gameObject.transform.parent = Selection.activeGameObject.transform;
                gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

            Selection.activeGameObject = gameObject;
            Undo.RegisterCreatedObjectUndo(gameObject, "Created Game Object");

           /* EditorApplication.delayCall += () =>
            {
                System.Type sceneHierarchyType = System.Type.GetType("UnityEditor.SceneHierarchyWindow.UnityEditor");
                EditorWindow.GetWindow(sceneHierarchyType).SendEvent(EditorGUIUtility.CommandEvent("Rename"));
            };*/

            return gameObject;
        }
    }
}