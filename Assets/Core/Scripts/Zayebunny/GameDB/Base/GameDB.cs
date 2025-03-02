using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Nocci.Zayebunny.GameDB.Base
{
    public static class GameDB
    {
        private static readonly Dictionary<Type, BaseModule> Modules = new();
        private static readonly Dictionary<Type, Dictionary<string, BaseObject>> Objects = new();

        private static GameDBSettings _settings;

        private static GameDBSettings Settings
        {
            get
            {
#if UNITY_EDITOR
                if (_settings != null) return _settings;
                var assetPath = EditorPrefs.GetString("GameDBSettingsPath", string.Empty);

                if (string.IsNullOrEmpty(assetPath))
                {
                    var guids = AssetDatabase.FindAssets("t:GameDBSettings");

                    if (guids.Length > 0)
                    {
                        assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                        EditorPrefs.SetString("GameDBSettingsPath", assetPath);
                    }
                }

                _settings = AssetDatabase.LoadAssetAtPath<GameDBSettings>(assetPath);

                return _settings;
#else
                return _settings;
#endif
            }
            set => _settings = value;
        } // ReSharper disable Unity.PerformanceAnalysis

        public static void Setup(GameDBSettings settings = null)
        {
            if(settings != null)
                Settings = settings;
            try
            {
                BuildDatabase();
                UnityEngine.Debug.Log("GameDB Setup");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
        }


        public static void BuildDatabase()
        {
            ClearDatabase();
            PopulateModules();
            PopulateObjects();
        }


        public static void ClearDatabase()
        {
            Modules.Clear();
            Objects.Clear();
        }

        public static T GetObject<T>(string key) where T : BaseObject
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return GetObjects<T>().FirstOrDefault(x => x.key == key);
#endif

            if (Objects.ContainsKey(typeof(T)) && Objects[typeof(T)].TryGetValue(key, out var obj))
                return obj as T;
            return null;
        }

        private static void PopulateObjects()
        {
            foreach (var keyValuePair in Modules)
            {
                var moduleType = keyValuePair.Key.BaseType;
                var module = keyValuePair.Value;
                if (moduleType.GetGenericArguments().Length == 0) continue;
                var type = moduleType.GetGenericArguments()[0];
                Objects.TryAdd(type, new Dictionary<string, BaseObject>());

                foreach (var obj in module.Objects)
                {
                    if (obj is null) continue;

                    if (string.IsNullOrEmpty(obj.key))
                    {
                        Debug.LogWarning("Object key is null in " + obj.Name + " of " + type + " module.");
                        continue;
                    }

                    Objects[type].TryAdd(obj.key, obj);
                }
            }
        }

        private static void PopulateModules()
        {
            var modules = Settings.GetModulesFlat();
            foreach (var module in modules)
            {
                if (module is null) continue;
                Modules.Add(module.GetType(), module);
            }

            foreach (var module in Modules) ClearModules(module.Value);
        }

        private static void ClearModules(BaseModule module)
        {
            for (var i = module.Objects.Count - 1; i >= 0; i--)
            {
                var obj = module.Objects[i];
                if (obj is null) module.Objects.RemoveAt(i);
            }

            for (var i = module.SubModules.Count - 1; i >= 0; i--)
            {
                var obj = module.SubModules[i];
                if (obj is null)
                {
                    module.SubModules.RemoveAt(i);
                    continue;
                }

                ClearModules(module.SubModules[i]);
            }
        }


        // ReSharper disable Unity.PerformanceAnalysis
        public static T GetModule<T>() where T : BaseModule
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return GetModuleRecursiveEditor<T>(Settings.Modules);
#endif

            return Modules.ContainsKey(typeof(T)) ? Modules[typeof(T)] as T : null;
        }

        public static IEnumerable<T> GetObjects<T>() where T : BaseObject
        {
#if UNITY_EDITOR

            if (!Application.isPlaying)
            {
                var modules = Settings.GetModulesFlat();
                var objects = new List<T>();
                foreach (var module in modules)
                {
                    if (module == null)
                        continue;

                    var moduleObjects = module.Objects.Where(x => x != null).ToList();

                    if (moduleObjects.Any(x => x.GetType() == typeof(T)))
                    {
                        objects.AddRange(moduleObjects.Cast<T>());
                        return objects;
                    }
                }

                return objects;
            }
#endif

            return Objects.ContainsKey(typeof(T))
                ? Objects[typeof(T)].Values.Cast<T>()
                : null;
        }

        public static IEnumerable<T> GetObjects<T>(this BaseModule module) where T : BaseObject
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return module.Objects.Cast<T>();
#endif

            if (module.SafeIsUnityNull())
                return new List<T>();

            return !Objects.ContainsKey(typeof(T)) ? new List<T>() : Objects[typeof(T)].Values.Cast<T>();
        }


        public static T GetObject<T>(this BaseModule module, string key) where T : BaseObject
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return module.Objects.FirstOrDefault(x => x.key == key) as T;
#endif
            if (module == null)
                return null;

            var type = typeof(T);

            return !Objects.ContainsKey(type) ? null : Objects[type].ContainsKey(key) ? Objects[type][key] as T : null;
        }


        /* public static IEnumerable<string> GetAllTags<T>(this T module) where T : BaseModule
         {
             if (module == null)
                 return new List<string>();

             var tags = new List<string>();

             foreach (var obj in module.Objects.Where(x => x != null))
             {
                 foreach (var tag in obj.tags.Where(tag => !tags.Contains(tag)))
                 {
                     tags.Add(tag);
                 }
             }

             foreach (var tag in module.SubModules.Select(subModule => subModule.GetAllTags())
                          .SelectMany(tagsSubmodule =>
                              tagsSubmodule.Where(tag => !tags.Contains(tag)).Where(x => x != null)))
             {
                 tags.Add(tag);
             }

             return tags;
         }*/


#if UNITY_EDITOR

        private static T GetModuleRecursiveEditor<T>(List<BaseModule> modules) where T : BaseModule
        {
            foreach (var module in modules)
            {
                if (module.GetType() == typeof(T)) return module as T;

                if (module.SubModules.Count <= 0) continue;
                var result = GetModuleRecursiveEditor<T>(module.SubModules);
                if (result != null) return result;
            }

            //Debug.LogError("Module not found");
            return null;
        }
#endif
    }
}