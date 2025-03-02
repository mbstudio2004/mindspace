using System;
using System.Collections;
using Eflatun.SceneReference;
using Nocci.Managers;
using Nocci.Zayebunny.GameDB.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nocci.Bootloader
{
    [DefaultExecutionOrder(-100)]
    public class Bootloader : MonoBehaviour
    {
        public SceneReference sceneReference;
        [Required] public GameDBSettings gameDbSettings;
        public static bool IsInitialized { get; private set; }

        public static Bootloader Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var app = Resources.Load<GameObject>("App");
            Instance = Instantiate(app).GetComponent<Bootloader>();
            DontDestroyOnLoad(Instance.gameObject);
        }

        private void Awake()
        {
            SetupManagers();
        }

        private void Start()
        {
            // StartCoroutine(LoadScene());
        }


        private void SetupManagers()
        {
/*
#if UNITY_EDITOR
            GameDB.Setup();
#else
            GameDB.Setup(gameDbSettings);
#endif
*/
            Debug.Log("Bootloader setup managers!");
            UpdateManager.Init();
            InputManager.Init();
            Debug.Log("Bootloader setup managers! 2");
            /*GameManager.Init();
            PoolingManager.Init();
            .Init();
            TweenManager.Init();
            SaveManager.Init();


            GameplaySettings.Init();
            PoolingManager.Init();

            TickManager.Instance.StartTick();
            GameManager.Init();
            AudioManager.Init();
            SurfaceManager.Init();
            LevelManager.Init();
            BuildingManager.Init();*/

            IsInitialized = true;
        }

        private IEnumerator LoadScene()
        {
            var getPath = "";

            try
            {
                getPath = sceneReference.Path;
            }
            catch (Exception)
            {
                //ignored
            }

            if (!string.IsNullOrEmpty(getPath))
            {
                var loadOperation = SceneManager.LoadSceneAsync(sceneReference.Path, LoadSceneMode.Single);
                loadOperation.completed += _ => { SceneManager.SetActiveScene(sceneReference.LoadedScene); };

                while (loadOperation.isDone) yield return null;

                Debug.Log("Bootloader loaded!");
            }
        }
    }
}