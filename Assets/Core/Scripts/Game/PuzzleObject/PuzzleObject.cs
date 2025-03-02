using System.Collections.Generic;
using Nocci.Managers;
using UnityEngine;

namespace Nocci
{
    [DisallowMultipleComponent]
    public class PuzzleObject : GuidComponent
    {
        public static List<PuzzleObject> AllPuzzleObjects { get; } = new();


        private readonly List<PuzzleObjectBehaviour> m_Behaviours = new();


        protected override void Awake()
        {
            base.Awake();
            GetComponentsInChildren(m_Behaviours);
            foreach (var behaviour in m_Behaviours)
            {
                behaviour.Initialize(this);
            }
        }

        private void OnEnable()
        {
            AllPuzzleObjects.Add(this);
            
            foreach (var behaviour in m_Behaviours)
            {
                behaviour.OnBehaviourEnabled();

                UpdateManager.AddUpdate(behaviour.OnUpdate);
                UpdateManager.AddFixedUpdate(behaviour.OnFixedUpdate);
            }
        }

        private void OnDisable()
        {
            AllPuzzleObjects.Remove(this);
            
            foreach (var behaviour in m_Behaviours)
            {
                behaviour.OnBehaviourDisabled();

                UpdateManager.RemoveUpdate(behaviour.OnUpdate);
                UpdateManager.RemoveFixedUpdate(behaviour.OnFixedUpdate);
            }
        }
    }
}