using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci.Zayebunny.GameDB.Base
{
    [Serializable]
    [InlineProperty]
    public struct GameDBReference<T> : IEquatable<GameDBReference<T>>
        where T : BaseObject
    {
        [SerializeField] [HideLabel] [InlineProperty] [ValueDropdown(nameof(Keys))]
        private string m_Value;
        
        private IEnumerable<string> Keys => GameDB.GetObjects<T>().Select(x => x.key);
        public T Data => GameDB.GetObject<T>(m_Value);
        public bool IsNull => string.IsNullOrEmpty(m_Value);

        public string Key => m_Value;
        public string Description => IsNull ? string.Empty : Data.Description;
        public string Name => IsNull ? string.Empty : Data.Name;
        public Sprite Icon => IsNull ? null : Data.icon;


        public static GameDBReference<T> Empty { get; } = new("");

        #region Constructors

        public GameDBReference(T def)
        {
            m_Value = def != null ? def.key : "";
        }

        public GameDBReference(string key)
        {
            m_Value = key;
        }

        #endregion

        #region Operators

        public static bool operator ==(GameDBReference<T> x, GameDBReference<T> y)
        {
            return x.m_Value == y.m_Value;
        }

        public static bool operator ==(GameDBReference<T> x, T y)
        {
            return y != null && x.m_Value == y.key;
        }

        public static bool operator ==(GameDBReference<T> x, string y)
        {
            return x.Key == y;
        }

        public static bool operator !=(GameDBReference<T> x, GameDBReference<T> y)
        {
            return x.m_Value != y.m_Value;
        }

        public static bool operator !=(GameDBReference<T> x, T y)
        {
            return y != null && x.m_Value != y.key;
        }

        public static bool operator !=(GameDBReference<T> x, string y)
        {
            return x.Key != y;
        }

        public static implicit operator GameDBReference<T>(string value)
        {
            return new GameDBReference<T>(value);
        }

        public static implicit operator string(GameDBReference<T> reference)
        {
            return reference.Key;
        }

        #endregion

        #region IEquatable Implementation

        public bool Equals(GameDBReference<T> other)
        {
            return m_Value == other.m_Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is GameDBReference<T>)
                return m_Value == ((GameDBReference<T>)obj).m_Value;
            if (obj is string)
                return m_Value == (string)obj;

            return false;
        }

        public override int GetHashCode()
        {
            return m_Value.GetHashCode();
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(m_Value))
                return Name;

            return string.Empty;
        }

        #endregion
    }

    public static class GameDBReferenceExtensions
    {
        public static bool ContainsObject<T>(this GameDBReference<T>[] thisDataRefArray, GameDBReference<T> dataRef)
            where T : BaseObject
        {
            if (thisDataRefArray == null)
                return false;

            for (var i = 0; i < thisDataRefArray.Length; i++)
                if (thisDataRefArray[i] == dataRef)
                    return true;

            return false;
        }

        public static bool ContainsAnyObject<T>(this GameDBReference<T>[] thisDataRefArray,
            GameDBReference<T>[] dataRefs)
            where T : BaseObject
        {
            if (thisDataRefArray == null)
                return true;

            for (var i = 0; i < thisDataRefArray.Length; i++)
            for (var j = 0; j < dataRefs.Length; j++)
                if (thisDataRefArray[i] == dataRefs[j])
                    return true;

            return false;
        }

        public static bool ContainsAnyObject<T>(this List<GameDBReference<T>> thisDataRefList,
            GameDBReference<T>[] dataRefs) where T : BaseObject
        {
            if (thisDataRefList == null)
                return false;

            for (var i = 0; i < dataRefs.Length; i++)
                if (thisDataRefList.Contains(dataRefs[i]))
                    return true;

            return false;
        }
    }
}