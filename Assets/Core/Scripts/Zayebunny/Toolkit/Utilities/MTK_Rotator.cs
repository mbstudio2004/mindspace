using UnityEngine;

namespace Nocci.Zayebunny.Utilities
{
    public class MTK_Rotator : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private Vector3 rotationAxis = Vector3.up;
        [SerializeField] private bool isLocalRotation;


        public float RotationSpeed
        {
            get => rotationSpeed;
            set => rotationSpeed = value;
        }

        private void Update()
        {
            if (isLocalRotation)
                transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime));
            else
                transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime), Space.World);
        }
    }
}