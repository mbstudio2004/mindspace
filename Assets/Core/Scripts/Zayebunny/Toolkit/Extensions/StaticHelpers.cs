using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nocci.Zayebunny.Extensions
{
    public static class StaticHelpers
    {
        public static Vector3 ScreenToWorld(this Camera camera, Vector3 position)
        {
            position.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(position);
        }

        public static Vector3 ClampDirectionOrientationY(Vector3 direction, float minAngle, float maxAngle)
        {
            var yRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            yRotation = Mathf.Clamp(yRotation, minAngle, maxAngle);
            var clampedRotation = Quaternion.Euler(0f, yRotation, 0f);
            var clampedShootingDirection = clampedRotation * Vector3.forward;
            return clampedShootingDirection;
        }

        public static void ParseCommandLineArguments(out Dictionary<string, string> providedArguments)
        {
            providedArguments = new Dictionary<string, string>();
            var args = Environment.GetCommandLineArgs();
            for (int current = 0, next = 1; current < args.Length; current++, next++)
            {
                var isFlag = args[current].StartsWith("-");
                if (!isFlag) continue;
                var flag = args[current].TrimStart('-');
                var flagHasValue = next < args.Length && !args[next].StartsWith("-");
                var value = flagHasValue ? args[next].TrimStart('-') : "";
                providedArguments.Add(flag, value);
            }
        }


        public static T DeepCopy<T>(T source) where T : class
        {
            // Check if the source object is null
            if (source == null)
                return null;

            // Create a memory stream and a binary formatter
            using var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();

            // Serialize the source object into the memory stream
            binaryFormatter.Serialize(memoryStream, source);

            // Rewind the memory stream and deserialize the object
            memoryStream.Position = 0;
            var deepCopy = (T)binaryFormatter.Deserialize(memoryStream);

            return deepCopy;
        }

        public static string ToCamelCase(this string str)
        {
            var words = Regex.Split(str, @"\W+");

            var result = "";
            foreach (var word in words)
                if (!string.IsNullOrEmpty(word))
                    result += char.ToUpper(word[0]) + word.Substring(1);

            if (!char.IsLetter(result[0])) result = "_" + result;

            return result;
        }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs)
            where T : class, IComparable<T>
        {
            var objects = new List<T>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)    
            {
                foreach (var type in
                         assembly.GetTypes()
                             .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
                    objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            
           

            //objects.Sort();
            return objects;
        }

        public static string StringifyNumber(this int number)
        {
            return number switch
            {
                >= 100000000 => (number / 1000000).ToString("#,0M"),
                >= 10000000 => (number / 1000000).ToString("0.#") + "M",
                >= 100000 => (number / 1000).ToString("#,0K"),
                >= 1000 => (number / 1000).ToString("0.#") + "K",
                _ => number.ToString("#,0")
            };
        }

        public static Component CopyComponent(this Component original, GameObject destination)
        {
            var type = original.GetType();
            var copy = destination.AddComponent(type);
            var fields = type.GetFields();
            foreach (var field in fields) field.SetValue(copy, field.GetValue(original));

            return copy;
        }

        public static Transform DestroyAllChildren(this Transform parent)
        {
            foreach (Transform child in parent) child.gameObject.SafeDestroy();

            return parent;
        }

        public static Transform SetActiveAllChildren(this Transform parent, bool value)
        {
            foreach (Transform child in parent) child.gameObject.SetActive(value);

            return parent;
        }

        public static Vector3 NormalizeAngles(this Vector3 angles)
        {
            angles.x = NormalizeAngle(angles.x);
            angles.y = NormalizeAngle(angles.y);
            angles.z = NormalizeAngle(angles.z);
            return angles;
        }

        public static float NormalizeAngle(this float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }

        public static T SafeDestroy<T>(this T obj) where T : Object
        {
            if (Application.isEditor && !Application.isPlaying)
                Object.DestroyImmediate(obj);
            else
                Object.Destroy(obj);

            return null;
        }

        public static T SafeDestroyGameObject<T>(this T component) where T : Component
        {
            if (component != null)
                SafeDestroy(component.gameObject);
            return null;
        }

        private static Transform _findInChildren(Transform trans, string name)
        {
            if (trans.name == name)
                return trans;

            for (var i = 0; i < trans.childCount; i++)
            {
                var found = _findInChildren(trans.GetChild(i), name);
                if (found != null)
                    return found;
            }

            return null;
        }

        public static Transform FindInChildren(this Transform trans, string name)
        {
            return _findInChildren(trans, name);
        }

        public static void WaitAFrame(this Action action)
        {
            _waitAFrame(action).Run();
        }

        public static void WaitForSeconds(this Action action, float delay)
        {
            _waitForSeconds(delay, action).Run();
        }

        private static IEnumerator _waitAFrame(Action action)
        {
            yield return null;
            action?.Invoke();
        }

        private static IEnumerator _waitForSeconds(float delay, Action action)
        {
            yield return delay.WaitForSeconds();
            action?.Invoke();
        }

        public static bool InRange(this int number, int min, int max)
        {
            return number >= min && number <= max;
        }

        public static bool InRange(this float number, float min, float max)
        {
            return number >= min && number <= max;
        }
    }
}