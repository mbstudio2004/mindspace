using System.Collections.Generic;
using Nocci.Zayebunny.Attributes;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Nocci.Zayebunny.Editor.Editor
{
    public class InlineStringListDrawer : OdinAttributeDrawer<InlineStringListAttribute, List<string>>
    {
        private bool focusNextTextField;
        private string newString = "";

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var list = ValueEntry.SmartValue;

            if (list == null)
                // EditorGUIHelper.ErrorMessageBox("The field must be initialized.");
                return;
            if (label != null)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 100; // Adjust this value to change the width of the label
            GUI.SetNextControlName("AddNewString");
            newString = EditorGUILayout.TextField("Add new string:", newString);

            if (focusNextTextField)
            {
                EditorGUI.FocusTextInControl("AddNewString");
                focusNextTextField = false;
            }

            if (Event.current.type == EventType.KeyDown &&
                (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter) &&
                !string.IsNullOrEmpty(newString))
            {
                list.Add(newString);
                newString = "";
                focusNextTextField = true;
                Event.current.Use(); // Consume the event so it doesn't trigger twice
            }

            EditorGUILayout.EndHorizontal();

            var availableWidth = EditorGUIUtility.currentViewWidth;
            var labelStyle = new GUIStyle(EditorStyles.label);
            var itemsPerRow = 0;

            // Calculate items per row based on the available width in the current Inspector window
            for (var i = 0; i < list.Count; i++)
            {
                var content = new GUIContent(list[i]);
                var width = labelStyle.CalcSize(content).x + 40; // Add 40 to account for the box and button
                if (width > availableWidth) break;
                itemsPerRow++;
                availableWidth -= (int)width;
            }

            itemsPerRow = Mathf.Max(1, itemsPerRow);

            var padding = 5f; // Adjust this value to change the padding inside the box
            GUILayout.Space(padding);

            var rowCount = list.Count == 0 ? 1 : Mathf.CeilToInt(list.Count / (float)itemsPerRow);

            for (var row = 0; row < rowCount; row++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(padding);
                for (var i = row * itemsPerRow; i < (row + 1) * itemsPerRow && i < list.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    var content = new GUIContent(list[i]);
                    var width = labelStyle.CalcSize(content).x;
                    EditorGUILayout.LabelField(content, GUILayout.Width(width));
                    if (GUILayout.Button("X", GUILayout.Width(15), GUILayout.Height(15))) list.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(padding);
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(padding);

            if (label != null) EditorGUILayout.EndVertical();

            ValueEntry.SmartValue = list;
        }
    }
}