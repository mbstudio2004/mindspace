using System;
using System.Collections.Generic;
using Nocci.Utils;
using UnityEngine;

namespace Nocci
{
    public class StateAttractOtherColor : PuzzleObjectBehaviour
    {
        public int stateToAttract;
        public SphereCollider AttractCollider;
        public float AttractForce = 10;
        public float threshold = 2f;
        public bool invert = false;
        private List<(PuzzleObjectGravity, PuzzleObjectState)> m_Objects = new();


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (TryGetComponent(out PuzzleObjectState state))
            {
                state.IsNeutral = true;
            }

            AttractCollider = gameObject.GetOrAddComponent<SphereCollider>();
            AttractCollider.isTrigger = true;
        }

#endif

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, threshold);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, AttractCollider.radius);
        }


        public override void OnFixedUpdate()
        {
            var listTorRemove = new List<(PuzzleObjectGravity, PuzzleObjectState)>();

            foreach (var (gravity, state) in m_Objects)
            {
                var distance = Vector3.Distance(state.transform.position, PuzzleObject.transform.position);
                if (distance >
                    AttractCollider.radius)
                {
                    listTorRemove.Add((gravity, state));
                }
                else if (state.State == stateToAttract)
                {
                    if (distance < threshold && !invert) continue;
                    gravity.Rigidbody.AddForce((PuzzleObject.transform.position - state.transform.position).normalized *
                                               AttractForce * (invert ? -1 : 1), ForceMode.Acceleration);
                }
                else
                {
                    listTorRemove.Add((gravity, state));
                }
            }

            foreach (var state in listTorRemove)
            {
                m_Objects.Remove(state);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var state = other.transform.GetComponentInParent<PuzzleObjectState>();
            if (state != null)
            {
                if (m_Objects.Exists(x => x.Item2 == state))
                    return;

                var gravity = state.GetComponent<PuzzleObjectGravity>();

                if (gravity != null && state.State == stateToAttract && !state.IsNeutral)
                    m_Objects.Add((gravity, state));
            }
        }

        private void OnTriggerStay(Collider other)
        {
            var state = other.transform.GetComponentInParent<PuzzleObjectState>();
            if (state != null)
            {
                if (m_Objects.Exists(x => x.Item2 == state))
                    return;


                var gravity = state.GetComponent<PuzzleObjectGravity>();

                if (gravity != null && state.State == stateToAttract && !state.IsNeutral)
                    m_Objects.Add((state.GetComponent<PuzzleObjectGravity>(), state));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var state = other.transform.GetComponentInParent<PuzzleObjectState>();
            if (state != null)
            {
                var gravity = state.GetComponent<PuzzleObjectGravity>();

                if (gravity != null && m_Objects.Exists(x => x.Item2 == state))
                    m_Objects.Remove((gravity, state));
            }
        }
    }
}