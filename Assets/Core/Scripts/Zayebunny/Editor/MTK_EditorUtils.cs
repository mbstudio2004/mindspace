using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Nocci.Zayebunny.Editor
{
    public static class MTK_EditorUtils
    {
        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets("t:" +
                                                 typeof(T)
                                                     .Name); //FindAssets uses tags check documentation for more info
            var a = new T[guids.Length];
            for (var i = 0; i < guids.Length; i++) //probably could get optimized 
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }

        public static bool SelectButtonList<T>(ref T selectedType, List<T> typesToDisplay) where T : Type
        {
            var rect = GUILayoutUtility.GetRect(0, 25);
            for (var i = 0; i < typesToDisplay.Count; i++)
            {
                var name = typesToDisplay[i].Name;
                var btnRect = rect.Split(i, typesToDisplay.Count);
                if (!SelectButton(btnRect, name, typesToDisplay[i] == selectedType)) continue;
                selectedType = typesToDisplay[i];
                return true;
            }

            return false;
        }

        public static bool SelectButton(Rect rect, string name, bool selected)
        {
            if (GUI.Button(rect, GUIContent.none, GUIStyle.none)) return true;

            if (Event.current.type != EventType.Repaint) return false;
            var style = new GUIStyle(EditorStyles.miniButtonMid)
            {
                stretchHeight = true,
                fixedHeight = rect.height
            };
            style.Draw(rect, GUIHelper.TempContent(name), false, false, selected, false);

            return false;
        }
    }
}