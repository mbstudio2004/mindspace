using System.Collections.Generic;
using UnityEngine;

namespace Nocci
{
    public class VentilatorMachine : PuzzleObjectMachineListener
    {
        public float OverrideGravity = 10;

        public bool invert = false;
        public float maxDistance = 10;
        public Vector3 direction = Vector3.up;
        public bool pushNeutralCubes = true;
        public Rotator Rotator;
        public PuzzleObjectState State;
        private List<(PuzzleObjectGravity, PuzzleObjectState)> m_Objects = new();

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            TryGetComponent(out State);
        }

#endif


        public override void OnFixedUpdate()
        {
            if (!Machine.IsRunning) return;

            var listTorRemove = new List<(PuzzleObjectGravity, PuzzleObjectState)>();

            foreach (var (gravity, state) in m_Objects)
            {
                if(gravity == null || state == null) continue;
                var distance = Vector3.Distance(state.transform.position, PuzzleObject.transform.position);
                if (distance >
                    maxDistance)
                {
                     
                    listTorRemove.Add((gravity, state));
                }
                else if (state.State == State.State)
                {
                    Debug.Log("VentilatorMachine: " + distance);
                    gravity.Rigidbody.AddForce(direction.normalized * OverrideGravity * (invert ? -1 : 1), ForceMode.Acceleration);
                   // gravity.GravityForce = OverrideGravity;
                }
                else
                {
                    listTorRemove.Add((gravity, state));
                }
            }

            foreach (var state in listTorRemove)
            {
                //state.Item1.GravityForce = -9.81f;
                m_Objects.Remove(state);
            }
        }

        protected override void OnMachineStop()
        {
            Rotator.enabled = false;
        }

        protected override void OnMachineStart()
        {
            Rotator.enabled = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            var state = other.transform.GetComponentInParent<PuzzleObjectState>();
            if (state != null)
            {
                if (m_Objects.Exists(x => x.Item2 == state))
                    return;

                var gravity = state.GetComponent<PuzzleObjectGravity>();

                if (gravity != null && (state.State == State.State && !state.IsNeutral) || pushNeutralCubes)
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
                
                if (gravity != null && (state.State == State.State && !state.IsNeutral) || pushNeutralCubes)
                    m_Objects.Add((gravity, state));
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