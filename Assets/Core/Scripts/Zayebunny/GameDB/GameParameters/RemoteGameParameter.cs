using System;
using System.Collections.Generic;
using Nocci.Zayebunny.GameDB.Base;
using Sirenix.OdinInspector;

namespace Nocci.Zayebunny.GameDB.GameParameters
{
    public class RemoteGameParameter
    {
        private static readonly Dictionary<string, GameParametersObject> CachedObjects = new();

        private readonly GameDBReference<GameParametersObject> _objectKey;
        private readonly string _propertyKey;
        private GameParametersObject _gameParameterObject;


        public RemoteGameParameter(string objectKey, string propertyKey)
        {
            _objectKey = objectKey;
            _propertyKey = propertyKey;
        }


        public bool IsStatic
        {
            get
            {
                GetGameParameterObject().GetData(_propertyKey, out var isStatic);
                return isStatic;
            }
        }


        private GameParametersObject GetGameParameterObject()
        {
            if (_gameParameterObject != null) return _gameParameterObject;
            if (CachedObjects.TryGetValue(_objectKey, out var gameParameterObject))
            {
                _gameParameterObject = gameParameterObject;
                return gameParameterObject;
            }


            try
            {
                if (_objectKey.IsNull) throw new Exception($"Game Parameter Object with key {_objectKey} not found");
                gameParameterObject = _objectKey.Data;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e.Message);
            }

            CachedObjects.Add(_objectKey, gameParameterObject);
            _gameParameterObject = gameParameterObject;
            return gameParameterObject;
        }

        public object GetValue()
        {
            //UnityEngine.Debug.Log( $"Getting value for {_objectKey} {_propertyKey}");
            var gameParameterObject = GetGameParameterObject();
            var prop = gameParameterObject.GetData(_propertyKey, out var isStatic);
            if (prop == null) return null;
            if (isStatic)
                return prop.Value;

            return MTK_GameParameters.GetParameter(gameParameterObject, _propertyKey);
        }

        public void SetValue(object value)
        {
            GetGameParameterObject().GetData(_propertyKey, out var isStatic);
            if (isStatic) return;

            var gameParameterObject = GetGameParameterObject();

            MTK_GameParameters.SetParameter(gameParameterObject, _propertyKey, value);
        }
    }

    [ReadOnly]
    [AttributeUsage(AttributeTargets.Field)]
    [FoldoutGroup("Parameters", order: 9999999, expanded: false)]
    [IncludeMyAttributes]
    public class SyncedWithRemoteGameParameterAttribute : Attribute
    {
        public SyncedWithRemoteGameParameterAttribute(string objectKey, string propertyKey)
        {
            ObjectKey = objectKey;
            PropertyKey = propertyKey;
        }

        public GameDBReference<GameParametersObject> ObjectKey { get; }
        public string PropertyKey { get; }
    }
}