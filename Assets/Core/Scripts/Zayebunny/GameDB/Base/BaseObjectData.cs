using System;
using System.Collections.Generic;
using System.Linq;
using Nocci.Zayebunny.Extensions;
using Sirenix.OdinInspector;

namespace Nocci.Zayebunny.GameDB.Base
{
    [Serializable]
    [InlineProperty]
    [HideReferenceObjectPicker]
    public abstract class BaseObjectData : IComparable<BaseObjectData>
    {
        public static IEnumerable<BaseObjectData> GetTypes => StaticHelpers.GetEnumerableOfType<BaseObjectData>();
        public static List<string> DataNames => GetTypes.Select(x => x.GetType().Name).ToList();


        public int CompareTo(BaseObjectData other)
        {
            return 0;
        }

        public abstract T GetData<T>(string key) where T : class;
    }
}