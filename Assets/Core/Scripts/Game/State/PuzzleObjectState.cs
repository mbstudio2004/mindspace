using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci
{
    [DisallowMultipleComponent]
    public class PuzzleObjectState : PuzzleObjectBehaviour
    {
        public int State
        {
            get => m_CurrentState;
            private set => SetState(value);
        }


        public event Action<int, int> OnStateChanged = delegate { };

        public static IEnumerable<PuzzleObjectState> AllStates =>
            PuzzleObject.AllPuzzleObjects.Select(x => x.GetComponentInChildren<PuzzleObjectState>());


        [HideInEditorMode] [ReadOnly, SerializeField]
        private int m_CurrentState;

        [Range(0, 5)] [DisableIf("@IsNeutral")] 
#if UNITY_EDITOR
        [OnValueChanged(nameof(Refresh))] 
#endif
        [DisableInPlayMode] [SerializeField]
        private int m_defaultState;

        [field: SerializeField] public bool IsNeutral { get; set; }


        private void Start()
        {
            SetState(m_defaultState);
        }

#if UNITY_EDITOR

        private void Refresh()
        {
            if (Application.isPlaying) return;
            var components = GetComponents<PuzzleObjectStateListener>();
            foreach (var listener in components)
            {
                listener.OnStateChanged(m_defaultState, m_defaultState);
            }
        }
#endif

        [Button]
        public void SetState(int state)
        {
            OnStateChanged?.Invoke(m_CurrentState, state);
            m_CurrentState = state;
        }
    }
}