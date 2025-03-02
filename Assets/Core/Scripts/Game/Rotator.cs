
using System;
using UnityEngine;

namespace Nocci
{
    public class Rotator : MonoBehaviour
    {
        public Transform target;
        public float speed;
        public Vector3 axis = Vector3.up;
        public ParticleSystem particles;

        private void OnEnable()
        {
            particles.Play();
        }
        
        private void OnDisable()
        {
            particles.Stop();
        }

        private void Update()
        {
            target.Rotate(axis, speed * Time.deltaTime);
        }
    }
}
