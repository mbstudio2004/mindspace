using System;
using System.Collections.Generic;
using System.Reflection;
using Nocci.Zayebunny.FastReflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nocci.Zayebunny.GameDB.GameParameters
{
    public static class RemoteGameParameterExtensions
    {
        private static readonly
            Dictionary<FieldInfo, Tuple<MemberGetter<object, object>, MemberSetter<object, object>>>
            CachedFieldSetters = new();

        private static readonly Dictionary<MonoBehaviour, List<Action<bool>>> SyncActions = new();

        private static readonly Dictionary<Tuple<string, string>, RemoteGameParameter> CachedRemoteGameParameters =
            new();

        public static void ClearSyncCache()
        {
            CachedFieldSetters.Clear();
            SyncActions.Clear();
            CachedRemoteGameParameters.Clear();
        }

        public static void SyncAllBehaviours()
        {
            var syncedBehaviours = Object.FindObjectsOfType<MonoBehaviour>();
            foreach (var syncedBehaviour in syncedBehaviours) syncedBehaviour.SyncRemoteGameParameters();
        }

        private static void SetupGameParameters(MonoBehaviour behaviour)
        {
            var fields = behaviour.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            SyncActions.TryAdd(behaviour, new List<Action<bool>>());
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<SyncedWithRemoteGameParameterAttribute>();
                if (attr == null) continue;

                var action = new Action<bool>(b =>
                    SyncRemoteGameParameter(behaviour, field, attr.ObjectKey, attr.PropertyKey, b));

                SyncActions[behaviour].Add(action);

                if (!CachedRemoteGameParameters.ContainsKey(new Tuple<string, string>(attr.ObjectKey,
                        attr.PropertyKey)))
                {
                    var remoteGameParameter = new RemoteGameParameter(attr.ObjectKey, attr.PropertyKey);
                    CachedRemoteGameParameters.Add(new Tuple<string, string>(attr.ObjectKey, attr.PropertyKey),
                        remoteGameParameter);
                }
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
                if (CachedFieldSetters.ContainsKey(field)) continue;

                var getter = field.DelegateForGet();
                var setter = field.DelegateForSet();

                CachedFieldSetters.Add(field,
                    new Tuple<MemberGetter<object, object>, MemberSetter<object, object>>(getter,
                        setter));
#endif
            }
        }

        private static void SyncRemoteGameParameter(object behaviour, FieldInfo field, string objectKey,
            string propertyKey,
            bool forceUseOfSave)
        {
            var remoteGameParameter = CachedRemoteGameParameters[new Tuple<string, string>(objectKey, propertyKey)];
            var value = remoteGameParameter.GetValue();

            if (value == null) return;


#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
if (remoteGameParameter.IsStatic || forceUseOfSave)
            {
                field.SetValue(behaviour, value);
            }
            else
            {
                var fieldValue = field.GetValue(behaviour);
                if (!value.Equals(fieldValue))
                {
                    remoteGameParameter.SetValue(fieldValue);
                }
                else
                {
                    field.SetValue(behaviour, value);
                }
            }
#else

            var getter = CachedFieldSetters[field].Item1;
            var setter = CachedFieldSetters[field].Item2;
            try
            {
                if (remoteGameParameter.IsStatic || forceUseOfSave)
                {
                    setter(ref behaviour, value);
                }
                else
                {
                    var fieldValue = getter(behaviour);
                    if (!value.Equals(fieldValue))
                        remoteGameParameter.SetValue(fieldValue);
                    else
                        setter(ref behaviour, value);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Remote Game Parameters: Error while syncing {objectKey} {propertyKey}.\n{e.Message}");
            }
#endif
        }

        public static void SyncRemoteGameParameters(this MonoBehaviour behaviour, bool forceUseOfSave = false)
        {
            if (!SyncActions.ContainsKey(behaviour)) SetupGameParameters(behaviour);

            foreach (var syncAction in SyncActions[behaviour]) syncAction(forceUseOfSave);
        }
    }
}