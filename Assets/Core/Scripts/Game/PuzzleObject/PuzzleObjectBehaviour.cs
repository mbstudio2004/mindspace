using System;
using UnityEngine;

namespace Nocci
{
    public abstract class PuzzleObjectBehaviour : MonoBehaviour
    {
        public PuzzleObject PuzzleObject { get; private set; }
        public bool IsObjectEnabled => PuzzleObject.enabled;

        public void Initialize(PuzzleObject puzzleObject)
        {
            PuzzleObject = puzzleObject;
        }

        private void Awake()
        {
            if (GetComponent<PuzzleObject>() == null && GetComponentInParent<PuzzleObject>() == null)
            {
                transform.root.gameObject.AddComponent<PuzzleObject>();
                Debug.LogWarning(
                    "PuzzleObjectBehaviour must be a child of a PuzzleObject. Adding PuzzleObject to root.");
            }
        }

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (GetComponent<PuzzleObject>() == null && GetComponentInParent<PuzzleObject>() == null)
            {
                transform.root.gameObject.AddComponent<PuzzleObject>();
                Debug.LogWarning(
                    "PuzzleObjectBehaviour must be a child of a PuzzleObject. Adding PuzzleObject to root.");
            }
        }

#endif


        public virtual void OnBehaviourEnabled()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnBehaviourDisabled()
        {
        }
    }
}