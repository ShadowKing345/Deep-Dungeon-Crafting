using UnityEngine;

namespace Project.Runtime
{
    public class CameraFollowScript : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [Range(0, 1f)] [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private Vector3 offset;

        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothSpeed);
        }
    }
}