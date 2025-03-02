using UnityEngine;

namespace Nocci.Zayebunny.Utilities
{
    public class MTK_FollowRotation : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool followXAxis = true;
        [SerializeField] private bool followYAxis = true;
        [SerializeField] private bool followZAxis = true;


        private void Update()
        {
            var targetRotation = target.rotation.eulerAngles;
            var rotation = transform.rotation.eulerAngles;

            if (followXAxis) rotation.x = targetRotation.x + offset.x;

            if (followYAxis) rotation.y = targetRotation.y + offset.y;

            if (followZAxis) rotation.z = targetRotation.z + offset.z;

            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}