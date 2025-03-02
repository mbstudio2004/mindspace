using Nocci.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci
{
    [DisallowMultipleComponent]
    public class PuzzleObjectGravity : PuzzleObjectBehaviour
    {
        [field: SerializeField] public bool IsGravityEnabled { get; private set; } = true;
        public Rigidbody Rigidbody => rigidbody;

        [SerializeField] private float gravityForce = 9.81f;
        [SerializeField] private Vector3 gravityVector = Vector3.down;

        [Space] [Header("Dependencies")] [Required] [SerializeField]
        private new Rigidbody rigidbody;

        public float GravityForce
        {
            get => gravityForce;
            set => gravityForce = value;
        }


#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
            if (rigidbody == null)
                rigidbody = transform.root.gameObject.GetOrAddComponent<Rigidbody>();

            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

#endif

        public override void OnUpdate()
        {
            if (IsGravityEnabled)
                rigidbody.AddForce(gravityVector * gravityForce * rigidbody.mass, ForceMode.Acceleration);
        }

        public void SetGravity(bool value)
        {
            IsGravityEnabled = value;
        }

        public void Freeze()
        {
            IsGravityEnabled = false;

            if (!rigidbody.isKinematic)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            rigidbody.isKinematic = true;
        }

        public void Unfreeze()
        {
            rigidbody.isKinematic = false;
            IsGravityEnabled = true;
        }
    }
}