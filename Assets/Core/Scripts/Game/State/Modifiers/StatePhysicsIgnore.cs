using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nocci.Modifiers
{
    public class StatePhysicsIgnore : PuzzleObjectStateListener
    {
        public bool ignorePhysicsOfOtherStates = true;

        private Collider _collider;
        private Dictionary<PuzzleObjectState, (Collider, StatePhysicsIgnore)> _cachedComponents = new();

        protected override void Awake()
        {
            _collider = GetComponent<Collider>();
            base.Awake();
        }

        public override void OnStateChanged(int oldState, int newState)
        {
            if (!ignorePhysicsOfOtherStates) return;

            var allStates = PuzzleObjectState.AllStates.ToList();
            for (var i = 0; i < allStates.Count; i++)
            {
                var state = allStates[i];
                if (state == m_State) continue;

                if (!_cachedComponents.TryGetValue(state, out var components))
                {
                    if (!state.TryGetComponent(out Collider colliderToIgnore)) continue;
                    state.TryGetComponent(out StatePhysicsIgnore ignore);
                    components = (colliderToIgnore, ignore);
                    _cachedComponents[state] = components;
                }

                if (state.IsNeutral || m_State.IsNeutral)
                {
                    Physics.IgnoreCollision(_collider, components.Item1, false);
                    continue;
                }

                if (components.Item2 != null && (!components.Item2.ignorePhysicsOfOtherStates))
                {
                    Physics.IgnoreCollision(_collider, components.Item1, false);
                    continue;
                }

                Physics.IgnoreCollision(_collider, components.Item1, state.State != newState);
            }
        }
    }
}