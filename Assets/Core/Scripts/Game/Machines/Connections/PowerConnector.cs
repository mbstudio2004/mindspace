using System;
using UnityEngine;

namespace Nocci
{
    public class PowerConnector : PuzzleObjectBehaviour
    {
        public bool isPowerSource;


        public Vector3 cableEndOffset = Vector3.zero;
        public Vector3 cableEndRotation = Vector3.zero;
        public float lerpSpeed = 0.5f;
        public float threshold = 2f;
        public SphereCollider sphereTrigger;

        private PuzzleCableEnd _cableEnd;
        private PuzzleObjectGravity _gravity;

        private PuzzleObjectState _state;

        public bool IsPowered
        {
            get => _isPowered;
            private set
            {
                if (_isPowered == value) return;
                _isPowered = value;
                Refresh();
            }
        }
        
        private void Update()
        {
            Debug.DrawLine(transform.position, transform.position + transform.up * 10, IsPowered ? Color.green : Color.red);
        }


        private bool _isPowered;

        public event Action<bool> OnPowerChanged = delegate { };


        private void Awake()
        {
            _state = GetComponent<PuzzleObjectState>();
        }

        private void Refresh()
        {
            OnPowerChanged.Invoke(IsPowered);
        }

        public void EnablePower()
        {
            IsPowered = true;
        }

        public void DisablePower()
        {
            IsPowered = false;
        }

        protected virtual void OnEnable()
        {
            PowerConnectionManager.AddConnector(this);
        }

        protected virtual void OnDisable()
        {
            PowerConnectionManager.RemoveConnector(this);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();


            if (_cableEnd == null || _gravity == null)
                return;

            _gravity.Freeze();
            
            var targetPos = transform.position + cableEndOffset;
            var targetRot = transform.rotation * Quaternion.Euler(cableEndRotation);

            _gravity.Rigidbody.MovePosition(Vector3.Lerp(_gravity.Rigidbody.position, targetPos,
                lerpSpeed * Time.fixedDeltaTime));
            _gravity.Rigidbody.MoveRotation(Quaternion.Lerp(_gravity.Rigidbody.rotation, targetRot,
                lerpSpeed * Time.fixedDeltaTime));
            if (Vector3.Distance(_cableEnd.transform.position, targetPos) > sphereTrigger.radius + threshold)
            {
                _cableEnd.Disconnect();
                _cableEnd = null;
                _gravity.Unfreeze();
                _gravity = null;
            }
        }

        protected virtual void OnDrawGizmos()
        {
            var targetPos = transform.position + cableEndOffset;
            var targetRot = transform.rotation * Quaternion.Euler(cableEndRotation);

            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(targetPos, threshold);
            Gizmos.DrawRay(targetPos, targetRot * Vector3.forward);
        }


        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PuzzleCableEnd connector))
            {
                if (connector == _cableEnd || _cableEnd != null)
                    return;

                var state = connector.GetComponent<PuzzleObjectState>();

                if (_state != null && state != null)
                {
                    if (_state.State != state.State)
                        return;
                }

                _gravity = connector.GetComponent<PuzzleObjectGravity>();
                _gravity.Freeze();
                _cableEnd = connector;

                _cableEnd.Connect(this);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out PuzzleCableEnd connector))
            {
                if (connector == _cableEnd || _cableEnd != null)
                    return;


                var state = connector.GetComponent<PuzzleObjectState>();

                if (_state != null && state != null)
                {
                    if (_state.State != state.State)
                        return;
                }

                _gravity = connector.GetComponent<PuzzleObjectGravity>();
                _gravity.Freeze();
                _cableEnd = connector;

                _cableEnd.Connect(this);
            }
        }
    }
}