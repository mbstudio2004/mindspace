using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace Nocci.Zayebunny.Editor
{
    [InitializeOnLoad]
    public static class ToolbarExtensions
    {
        private const string ZB_ICON_PATH = "Assets/_Core/Scripts/Zayebunny/Editor/Resources/zayebunnyWhite.png";

        static ToolbarExtensions()
        {
            ToolbarExtender.RightToolbarGUI.Add(DrawLeftGUI);
        }

        private static void DrawLeftGUI()
        {
            GUILayout.Space(15);
            DrawZayebunny();
           GUILayout.FlexibleSpace();
        }

        private static void DrawZayebunny()
        {
            //var icon = EditorResources.Load<Texture>("zayebunnyWhite", isRequired:false);
            var icon = EditorGUIUtility.Load(ZB_ICON_PATH) as Texture;
            if (GUILayout.Button(icon, GUILayout.Width(32), GUILayout.Height(20))) GameDBEditor.OpenWindow();
        }
    }
}