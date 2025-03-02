using UnityEngine;

namespace Nocci.Zayebunny.Extensions
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance;
        public static bool HasInstance => _instance != null;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();
                if (_instance != null) return _instance;
                var obj = new GameObject("Singleton of " + typeof(T).Name);
                _instance = obj.AddComponent<T>();
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying) return;

            _instance = this as T;
        }
    }
}