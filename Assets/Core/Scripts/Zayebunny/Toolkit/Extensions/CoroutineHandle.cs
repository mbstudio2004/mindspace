using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = System.Object;

namespace Nocci.Zayebunny.Extensions
{
    public static class CoroutineExtensions
    {
        private const HideFlags HIDE_FLAGS = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy |
                                             HideFlags.HideInInspector | HideFlags.NotEditable |
                                             HideFlags.DontSaveInBuild;

        private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsCache =
            new(new FloatEqualityComparer());

        private static CoroutineRunner Operation;

        public static WaitForSeconds WaitForSeconds(this float seconds)
        {
            if (!_waitForSecondsCache.TryGetValue(seconds, out var waitForSeconds))
            {
                waitForSeconds = new WaitForSeconds(seconds);
                _waitForSecondsCache.Add(seconds, waitForSeconds);
            }

            return waitForSeconds;
        }

        public static Coroutine Run(this IEnumerator iEnumerator)
        {
            if (Operation != null) return Operation.StartCoroutine(iEnumerator);
            Operation = new GameObject("[CoroutineRunner]").AddComponent<CoroutineRunner>();
            Operation.hideFlags = HIDE_FLAGS;
            Operation.gameObject.hideFlags = HIDE_FLAGS;
            return Operation.StartCoroutine(iEnumerator);
        }

        public static CoroutineHandle RunWithHandler(this IEnumerator iEnumerator,
            Action<CoroutineHandle> completed = null)
        {
            if (Operation != null) return new CoroutineHandle(Operation, iEnumerator, completed);
            Operation = new GameObject("[CoroutineRunner]").AddComponent<CoroutineRunner>();
            Operation.hideFlags = HIDE_FLAGS;
            Operation.gameObject.hideFlags = HIDE_FLAGS;
            return new CoroutineHandle(Operation, iEnumerator, completed);
        }

        public static void Stop(this Coroutine coroutine)
        {
            if (Operation != null) Operation.StopCoroutine(coroutine);
        }


        [ExecuteInEditMode]
        public class CoroutineRunner : MonoBehaviour
        {
            ~CoroutineRunner()
            {
                gameObject.SafeDestroy();
            }
        }
    }


    public class CoroutineHandle : IEnumerator
    {
        public CoroutineHandle(MonoBehaviour owner, IEnumerator coroutine, Action<CoroutineHandle> completed = null)
        {
            if (completed != null) Completed += completed;

            Current = owner.StartCoroutine(Wrap(coroutine));
        }

        public bool IsDone { get; private set; }

        public bool MoveNext()
        {
            return !IsDone;
        }

        public object Current { get; }

        public void Reset()
        {
        }

        public event Action<CoroutineHandle> Completed;

        private IEnumerator Wrap(IEnumerator coroutine)
        {
            yield return coroutine;
            IsDone = true;
            Completed?.Invoke(this);
        }
    }
}