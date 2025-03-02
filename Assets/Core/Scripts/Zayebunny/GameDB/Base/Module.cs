using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Nocci.Zayebunny.GameDB.Base
{
    /// <summary>
    /// Represents a module that contains objects of type T.
    /// </summary>
    public abstract class Module<T> : BaseModule where T : BaseObject
    {
        /// <summary>
        /// Gets the type of the objects contained in the module.
        /// </summary>
        public override Type InheritorType { get; protected set; } = typeof(T);

#if UNITY_EDITOR
        /// <summary>
        /// Adds a new object to the module.
        /// </summary>
        /// <param name="displayName">The display name of the object. If not provided, a default name is used.</param>
        /// <returns>The newly created object.</returns>
        public override BaseObject AddObject(string displayName = "")
        {
            var name = string.IsNullOrEmpty(displayName) ? "New Object " + GetObjectCount() : displayName;
            var obj = CreateInstance<T>();
            obj.Name = name;
            
            OnPreAddObject(obj);

           /* var directory = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
            var newDirectory = System.IO.Path.Combine(directory, this.DisplayName);

            if (!System.IO.Directory.Exists(newDirectory))
            {
                System.IO.Directory.CreateDirectory(newDirectory);
            }

            var assetPath = System.IO.Path.Combine(newDirectory, $"({this.DisplayName}) {name}.asset");
            AssetDatabase.CreateAsset(obj, assetPath);*/

           var module = this;
           obj.name = name;
           AssetDatabase.AddObjectToAsset( obj, module);
           AssetDatabase.SaveAssetIfDirty(module);
           
            Objects.Add(obj);
            
            OnPostAddObject(obj);
            return obj;
        }

        /// <summary>
        /// Adds an existing object to the module.
        /// </summary>
        /// <param name="obj">The object to add.</param>
        /// <returns>The added object, or null if the object is not of the correct type.</returns>
        public override BaseObject AddObject(BaseObject obj)
        {
            if (!obj.GetType().IsSubclassOf(typeof(T))) return null;
            Objects.Add(obj);
            return obj;
        }
#endif
    }
}