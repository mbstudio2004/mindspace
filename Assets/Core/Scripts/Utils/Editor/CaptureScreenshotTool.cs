using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Nocci.Utils.Editor
{
    public static class CaptureScreenshotTool
    {
        public const string k_editorPref = "CaptureScreenshotPath";
        public const string k_settingsPath = "Project/Capture Screenshot";

        [MenuItem(k_settingsPath + " _F12")]
        public static void CaptureScreenshotToolMenuItem()
        {
            var path = EditorPrefs.GetString(k_editorPref);
            if (string.IsNullOrWhiteSpace(path))
                path = GetDefaultPath();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var filename = Path.Combine(path, $"{Application.productName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png");

            ScreenCapture.CaptureScreenshot(filename, 1);
            
            Debug.Log($"Screenshot saved to: {filename}");
            
            EditorUtility.RevealInFinder(filename);
        }


        public static string GetDefaultPath()
        {
            string defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "Screenshots");
            return defaultPath;
        }
    }


    public class CaptureScreenshotSettingsProvider : SettingsProvider
    {
        public CaptureScreenshotSettingsProvider(string path, SettingsScope scope = SettingsScope.User) : base(path,
            scope)
        {
        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            GUILayout.Space(20f);
            var path = EditorPrefs.GetString(CaptureScreenshotTool.k_editorPref);

            if (string.IsNullOrWhiteSpace(path))
                path = CaptureScreenshotTool.GetDefaultPath();

            var changedPath = EditorGUILayout.TextField(path);
            if (string.CompareOrdinal(path, changedPath) != 0)
            {
                EditorPrefs.SetString(CaptureScreenshotTool.k_editorPref, changedPath);
            }

            GUILayout.Space(10f);

            if (GUILayout.Button("Reset to Default", GUILayout.Width(150f)))
            {
                EditorPrefs.DeleteKey(CaptureScreenshotTool.k_editorPref);
                Repaint();
            }
        }

        [SettingsProvider]
        public static SettingsProvider CreateCaptureScreenshotSettingsProvider()
        {
            var captureScreenshotSettingsProvider =
                new CaptureScreenshotSettingsProvider(CaptureScreenshotTool.k_settingsPath);
            return captureScreenshotSettingsProvider;
        }
    }
}