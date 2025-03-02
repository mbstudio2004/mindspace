using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEditorInternal;
using UnityEngine;


namespace Nocci.Utils.Editor
{
    public static class LayoutSwitcherTool
    {
        [Shortcut("LayoutSwitcher/Layout1", KeyCode.Keypad1, ShortcutModifiers.Control)]
        public static void Layout1MenuItem()
        {
            OpenLayout(0);
        }

        [Shortcut("LayoutSwitcher/Layout2", KeyCode.Keypad2, ShortcutModifiers.Control)]
        public static void Layout2MenuItem()
        {
            OpenLayout(1);
        }

        [Shortcut("LayoutSwitcher/Layout3", KeyCode.Keypad3, ShortcutModifiers.Control)]
        public static void Layout3MenuItem()
        {
            OpenLayout(2);
        }

        [Shortcut("LayoutSwitcher/Layout4", KeyCode.Keypad4, ShortcutModifiers.Control)]
        public static void Layout4MenuItem()
        {
            OpenLayout(3);
        }

        [Shortcut("LayoutSwitcher/Layout5", KeyCode.Keypad5, ShortcutModifiers.Control)]
        public static void Layout5MenuItem()
        {
            OpenLayout(4);
        }

        [Shortcut("LayoutSwitcher/Layout6", KeyCode.Keypad6, ShortcutModifiers.Control)]
        public static void Layout6MenuItem()
        {
            OpenLayout(5);
        }

        [Shortcut("LayoutSwitcher/Layout7", KeyCode.Keypad7, ShortcutModifiers.Control)]
        public static void Layout7MenuItem()
        {
            OpenLayout(6);
        }

        [Shortcut("LayoutSwitcher/Layout8", KeyCode.Keypad8, ShortcutModifiers.Control)]
        public static void Layout8MenuItem()
        {
            OpenLayout(7);
        }

        [Shortcut("LayoutSwitcher/Layout9", KeyCode.Keypad9, ShortcutModifiers.Control)]
        public static void Layout9MenuItem()
        {
            OpenLayout(8);
        }

        

        static bool OpenLayout(int index)
        {
            if (Application.isPlaying) return false;
            var path = GetWindowLayoutPath(index);
            if (string.IsNullOrWhiteSpace(path))
                return false;

            var windowLayoutType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.WindowLayout");

            if (windowLayoutType != null)
            {
                var tryLoadWindowLayoutMethod = windowLayoutType.GetMethod("LoadWindowLayout",
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new[] { typeof(string), typeof(bool) },
                    null);

                if (tryLoadWindowLayoutMethod != null)
                {
                    var arguments = new object[] { path, false };
                    var result = (bool)tryLoadWindowLayoutMethod.Invoke(null, arguments);
                    return result;
                }
            }

            return false;
        }
        

        static string GetWindowLayoutPath(int index)
        {
            var layoutsPreferencesPath = Path.Combine(InternalEditorUtility.unityPreferencesFolder, "Layouts");
            var layoutsModePreferencesPath = Path.Combine(layoutsPreferencesPath, ModeService.currentId);

            if (Directory.Exists(layoutsModePreferencesPath))
            {
                var layoutPaths = Directory.GetFiles(layoutsModePreferencesPath, "*.wlt", SearchOption.AllDirectories);
                if (index < layoutPaths.Length)
                    return layoutPaths[index];
            }

            return null;
        }
    }
}