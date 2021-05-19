using System;
using Entity;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 7.5f;
    
        public Rigidbody2D rb;
    
        private Vector2 _movement;

        private EntityAnimation _animatior;
        public Vector2 direction;

        private void Start()
        {
            _animatior = _animatior != null ? _animatior : GetComponent<EntityAnimation>();
        }

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
            
            // Plays Animation.
            _animatior.SetDirection(_movement);
            if (_movement.magnitude > 0.0f) direction = _movement;
        }
    }
}
