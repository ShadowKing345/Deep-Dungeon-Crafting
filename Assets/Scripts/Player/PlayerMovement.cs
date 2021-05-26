using System;
using Entity;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 7.5f;
    
        public Rigidbody2D rb;
        public EntityAnimator animator;
    
        private Vector2 _movement;

        public Vector2 Direction => _movement.normalized;

        private void Start()
        {
            rb ??= GetComponent<Rigidbody2D>();
            animator ??= GetComponent<PlayerAnimator>().bodyAnimator;
        }

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _movement.Normalize();
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
            
            // Plays Animation.
            animator.direction = _movement;
            animator.isMoving = _movement.magnitude > 0.0f;
        }
    }
}
