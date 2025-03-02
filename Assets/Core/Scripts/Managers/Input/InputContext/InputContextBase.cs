using System;
using UnityEngine;
using UnityEngine.Events;

namespace Nocci.Input
{
    public abstract class InputContextBase : ScriptableObject
    {
        [field: NonSerialized] public bool IsEnabled { get; private set; }

        public event UnityAction ContextEnabled;
        public event UnityAction ContextDisabled;


        internal virtual void EnableContext()
        {
            if (IsEnabled)
                return;

            IsEnabled = true;
            ContextEnabled?.Invoke();
        }

        internal virtual void DisableContext(InputContextBase newContext)
        {
            if (!IsEnabled)
                return;

            IsEnabled = false;
            ContextDisabled?.Invoke();
        }

        internal abstract InputContextBase[] GetSubContexts();

        internal bool ContainsSubContext(InputContextBase context)
        {
            if (this == context)
                return true;

            var subContexts = GetSubContexts();
            foreach (var ctx in subContexts)
                if (ctx == context)
                    return true;

            return false;
        }
    }
}