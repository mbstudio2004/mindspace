using System.Linq;
using Eflatun.SceneReference;
using GameKit.Dependencies.Utilities;
using Nocci.Game;
using QFSW.QC.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;


namespace Nocci
{
    [InitializeOnLoad]
    public static class StartFromThisSceneToolbar
    {
        private static string OriginalScenePath;
        private static SceneReference CurrentGameScene;
        private static GameManager.GameType type;

        static StartFromThisSceneToolbar()
        {
            ToolbarExtender.LeftToolbarGUI.Add(DrawLeftGUI);
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void DrawLeftGUI()
        {
            GUILayout.FlexibleSpace();

            if (!EditorApplication.isPlaying)
            {
                type = (GameManager.GameType)EditorGUILayout.EnumPopup(type, GUILayout.Width(110),
                    GUILayout.Height(20));
            }

            if (!EditorApplication.isPlaying &&
                GUILayout.Button("Start From This Scene", GUILayout.Width(150), GUILayout.Height(20)))
            {
                // Save the currently active scene to load it back after exiting play mode
                OriginalScenePath = SceneManager.GetActiveScene().path;

                // Assuming the bootloader scene is always at build index 0
                var bootloaderScenePath = SceneUtility.GetScenePathByBuildIndex(0);
                SessionState.SetString("Session", OriginalScenePath);
                SessionState.SetString("GameType", type.ToString());

                var scenes = EditorBuildSettings.scenes.ToList();
                scenes.Add(new EditorBuildSettingsScene(OriginalScenePath, true));
                EditorBuildSettings.scenes = scenes.DistinctBy(x => x.guid).ToArray();

                var gameMode = Object.FindObjectOfType<GameMode.GameMode>();
                if (gameMode == null)
                {
                    var gameModePrefab = Resources.Load<GameObject>("Gamemode");
                    PrefabUtility.InstantiatePrefab(gameModePrefab);
                }

                // Load the bootloader scene and start play mode
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(bootloaderScenePath);
                EditorApplication.EnterPlaymode();
            }
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            OriginalScenePath = SessionState.GetString("Session", OriginalScenePath);

            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(OriginalScenePath);
            var currentActiveScene = new SceneReference(sceneAsset);
            CurrentGameScene = currentActiveScene;

            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                GameRunTest.GameType = (GameManager.GameType)System.Enum.Parse(typeof(GameManager.GameType),
                    SessionState.GetString("GameType", "SinglePlayer"));
                GameRunTest.GameScene = CurrentGameScene;
            }

            if (state == PlayModeStateChange.EnteredEditMode)
            {
                // After exiting play mode, load back the original scene
                if (!string.IsNullOrEmpty(OriginalScenePath))
                {
                    EditorSceneManager.OpenScene(OriginalScenePath);
                }
            }
        }
    }
}