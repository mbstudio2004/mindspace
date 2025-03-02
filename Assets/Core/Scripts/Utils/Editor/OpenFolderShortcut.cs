using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Nocci.Utils.Editor
{
    public class OpenFolderShortcut
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId)
        {
            var e = Event.current;
            if (e == null || !e.control)
                return false;

            var obj = EditorUtility.InstanceIDToObject(instanceId);
            var path = AssetDatabase.GetAssetPath(obj);
            if (AssetDatabase.IsValidFolder(path))
            {
                EditorUtility.RevealInFinder(path);
            }

            return true;
        }
    }
}