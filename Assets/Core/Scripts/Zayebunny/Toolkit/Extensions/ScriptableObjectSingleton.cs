using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci.Zayebunny.Extensions
{
    public class ScriptableObjectSingleton<T> : SerializedScriptableObject where T : ScriptableObjectSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var sObjects = Resources.LoadAll<T>("");
                    if (sObjects == null || sObjects.Length == 0)
                        UnityEngine.Debug.LogError($"No ScriptableObject of type {typeof(T)} found in Resources folder.");
                    if (sObjects.Length > 1)
                        UnityEngine.Debug.LogError(
                            $"More than one ScriptableObject of type {typeof(T)} found in Resources folder.");

                    if (sObjects[0] != null)
                        _instance = sObjects[0];
                    else
                        UnityEngine.Debug.LogError($"No ScriptableObject of type {typeof(T)} found in Resources folder.");
                }

                return _instance;
            }
        }
    }
}