using UnityEngine;

namespace Entity.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public float moveSpeed = 7.5f;
    
        public Rigidbody2D rb;
    
        private Vector2 _movement;
        private Vector2 _mousePos;
        
        public Animator animator;
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            // todo: set direction.
            animator.SetFloat(Horizontal, _movement.x);
            animator.SetFloat(Vertical, _movement.y);
            animator.SetFloat( Speed, _movement.sqrMagnitude);
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}
