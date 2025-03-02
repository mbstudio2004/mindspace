using System;
using Sirenix.OdinInspector;
using UnityEngine;

//using SimpleJSON;

namespace Nocci.Zayebunny.GameDB.GameParameters
{
    [Serializable]
    public class GameParameter
    {
        [HorizontalGroup(200)] [HideLabel] public string key;

        [HorizontalGroup(200)] [HideLabel] public GameParameterType type;

        [HideLabel] [HorizontalGroup] [ShowIf(nameof(type), GameParameterType.Bool)]
        public bool boolValue;

        [HorizontalGroup] [HideLabel] [ShowIf(nameof(type), GameParameterType.Int)]
        public int intValue;

        [HideLabel] [HorizontalGroup] [ShowIf(nameof(type), GameParameterType.Float)]
        public float floatValue;

        [HideLabel] [HorizontalGroup] [ShowIf(nameof(type), GameParameterType.String)]
        public string stringValue;

        [HideLabel] [HorizontalGroup] [ShowIf(nameof(type), GameParameterType.Vector2)]
        public Vector2 vector2Value;

        [HideLabel] [HorizontalGroup] [ShowIf(nameof(type), GameParameterType.Vector3)]
        public Vector3 vector3Value;

        [HideLabel] [HorizontalGroup] [ShowIf(nameof(type), GameParameterType.Quaternion)]
        public Quaternion quaternionValue;

        [HorizontalGroup] [HideLabel] [ShowIf(nameof(type), GameParameterType.Color)]
        public Color colorValue;

        public object Value
        {
            get
            {
                return type switch
                {
                    GameParameterType.Bool => boolValue,
                    GameParameterType.Int => intValue,
                    GameParameterType.Float => floatValue,
                    GameParameterType.String => stringValue,
                    GameParameterType.Vector2 => vector2Value,
                    GameParameterType.Vector3 => vector3Value,
                    GameParameterType.Quaternion => quaternionValue,
                    GameParameterType.Color => colorValue,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            private set
            {
                if (type == GameParameterType.Bool)
                    boolValue = (bool)value;
                else if (type == GameParameterType.Int)
                    intValue = (int)value;
                else if (type == GameParameterType.Float)
                    floatValue = (float)value;
                else if (type == GameParameterType.String)
                    stringValue = (string)value;
                else if (type == GameParameterType.Vector2)
                    vector2Value = (Vector2)value;
                else if (type == GameParameterType.Vector3)
                    vector3Value = (Vector3)value;
                else if (type == GameParameterType.Quaternion)
                    quaternionValue = (Quaternion)value;
                else if (type == GameParameterType.Color)
                    colorValue = (Color)value;
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        public T GetValue<T>()
        {
            return (T)Convert.ChangeType(Value, typeof(T));
        }

        public void SetValue<T>(T value)
        {
            Value = value;
        }
    }
}