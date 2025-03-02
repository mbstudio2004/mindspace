using UnityEngine;

namespace Nocci.Modifiers
{
    public class StateChangeOtherColor : PuzzleObjectBehaviour
    {
        public int stateToChange;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (TryGetComponent(out PuzzleObjectState state))
            {
                state.IsNeutral = true;
            }
        }

#endif
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PuzzleObjectState state))
            {
                if (state.State != stateToChange && !state.IsNeutral)
                    state.SetState(stateToChange);
            }
        }
    }
}