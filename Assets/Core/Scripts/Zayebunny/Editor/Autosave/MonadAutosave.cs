using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Nocci
{
    [InitializeOnLoad]
    public static class MonadAutosave
    {
        static MonadAutosave()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
            {
                Save();
            }
        }


        private static void Save()
        {
            if (!Application.isPlaying) return;
            Debug.Log($"Auto-saved at {DateTime.Now:h:mm:ss tt}");
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
}