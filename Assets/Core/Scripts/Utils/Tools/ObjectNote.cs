#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Nocci.Extensions
{
    public class ObjectNote : MonoBehaviour
    {
        public bool readOnly = true;

        [DisableIf(nameof(readOnly))] public string shortNote;

        [DisableIf(nameof(readOnly))] [TextArea(20, 50)]
        public string longNote =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.\"";

        private void OnDrawGizmos()
        {
            var guiContent = new GUIContent(shortNote);
            var style = new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter };
            style.fontStyle = FontStyle.Bold;
            Handles.Label(transform.position, guiContent, style);
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(shortNote))
                shortNote = gameObject.name;
        }

    }
}
#endif