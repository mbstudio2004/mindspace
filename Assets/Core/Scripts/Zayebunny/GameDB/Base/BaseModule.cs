using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Nocci.Zayebunny.GameDB.Base
{
    /// <summary>
    /// Base class for all modules in the game.
    /// </summary>
    public abstract class BaseModule : ScriptableObject, IJsonSerializable
    {
        /// <summary>
        /// Display name of the module.
        /// </summary>
        [FoldoutGroup("Module settings", false, AnimateVisibility = true)]
        public string DisplayName;

        /// <summary>
        /// List of objects associated with the module.
        /// </summary>
        [FoldoutGroup("Module settings")] [SerializeReference]
        public List<BaseObject> Objects = new();

        /// <summary>
        /// List of submodules associated with the module.
        /// </summary>
        [FoldoutGroup("Module settings")] [SerializeReference]
        public List<BaseModule> SubModules = new();

        /// <summary>
        /// Color of the module.
        /// </summary>
        [FoldoutGroup("Module settings")] [SerializeField]
        private Color color = Color.blue;

        /// <summary>
        /// Indicates if the module is a tree.
        /// </summary>
        [FoldoutGroup("Module settings")] public bool IsTree;

        /// <summary>
        /// Type of the inheritor of the module.
        /// </summary>
        public virtual Type InheritorType { get; protected set; }

        /// <summary>
        /// Indexer to get object by key.
        /// </summary>
        public BaseObject this[string key] => GetObject(key);

        /// <summary>
        /// Gets the count of objects in the module.
        /// </summary>
        public virtual int GetObjectCount()
        {
            return Objects.Count;
        }

        /// <summary>
        /// Gets an object by key.
        /// </summary>
        public BaseObject GetObject(string key)
        {
            return this.GetObject<BaseObject>(key);
        }

        /// <summary>
        /// Gets a submodule by name.
        /// </summary>
        public virtual BaseModule GetSubModule(string name)
        {
            return SubModules.Find(x => x.DisplayName == name);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Adds a submodule to the module.
        /// </summary>
        public virtual void AddSubModule(BaseModule module)
        {
            SubModules.Add(module);
        }

        /// <summary>
        /// Removes a submodule from the module.
        /// </summary>
        public virtual void RemoveSubModule(BaseModule module)
        {
            SubModules.Remove(module);
        }

        /// <summary>
        /// Adds an object to the module.
        /// </summary>
        public virtual BaseObject AddObject(string displayName = "")
        {
            return default;
        }

        public virtual void OnPreAddObject(BaseObject obj)
        {
        }

        public virtual void OnPostAddObject(BaseObject obj)
        {
        }

        /// <summary>
        /// Adds an object to the module.
        /// </summary>
        public virtual BaseObject AddObject(BaseObject obj)
        {
            Objects.Add(obj);
            return obj;
        }

        /// <summary>
        /// Removes an object from the module.
        /// </summary>
        public virtual void RemoveObject(BaseObject obj)
        {
            if (obj.IsStatic)
                return;
            Objects.Remove(obj);
            AssetDatabase.RemoveObjectFromAsset(obj);
        }
#endif
    }
}