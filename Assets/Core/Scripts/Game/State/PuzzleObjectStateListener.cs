using UnityEngine;

namespace Nocci
{
    public abstract class PuzzleObjectStateListener : MonoBehaviour
    {
        protected PuzzleObjectState m_State { get; private set; }

        protected virtual void Awake()
        {
            m_State = GetComponentInParent<PuzzleObjectState>();
            m_State.OnStateChanged += OnStateChanged;
        }

        public abstract void OnStateChanged(int oldState, int newState);
    }
}