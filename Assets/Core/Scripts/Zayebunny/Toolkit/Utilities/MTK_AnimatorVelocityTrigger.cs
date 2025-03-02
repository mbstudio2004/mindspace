using UnityEngine;

namespace Nocci.Zayebunny.Utilities
{
    public class MTK_AnimatorVelocityTrigger : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string parameterName;
        [SerializeField] private float threshold = 0.1f;
        [SerializeField] private float velocityMultiplier = 1f;
        [SerializeField] private new Rigidbody rigidbody;

        private void Update()
        {
            var velocity = rigidbody.velocity.magnitude * velocityMultiplier;
            var trigger = velocity > threshold;
            animator.SetBool(parameterName, trigger);
        }
    }
}