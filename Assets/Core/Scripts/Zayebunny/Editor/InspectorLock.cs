namespace Nocci.Editor
{
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    public class LockInspector {
        
        [MenuItem("Edit/Lock Inspector %q")]
        public static void Lock () {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;

            foreach (var activeEditor in ActiveEditorTracker.sharedTracker.activeEditors) {
                if (activeEditor.target is not Transform) continue;
            
                var transform = (Transform) activeEditor.target;
            
                var propInfo = transform.GetType().GetProperty("constrainProportionsScale", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
            
                if (propInfo == null) continue;
            
                var value = (bool) propInfo.GetValue(transform, null);
                propInfo.SetValue(transform, !value, null);
            }

            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }

        [MenuItem("Edit/Lock Inspector %q", true)]
        public static bool Valid() {
            return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
        }

    }
}
