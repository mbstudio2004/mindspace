using System;
using Nocci.Interaction;
using Nocci.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci.Pickuping
{
    public class PickupableObject : Interactable
    {
        [ShowInInspector] public Rigidbody Rigidbody { get; set; }
        [ShowInInspector] public PuzzleObjectGravity gravity;
        public Collider Collider { get; private set; }

        private void Awake()
        {
            Rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            Collider = GetComponentInChildren<Collider>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Rigidbody == null)
            {
                Rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            }
            
            if (Collider == null)
            {
                Collider = GetComponentInChildren<Collider>();
            }
        }

#endif
    }
}