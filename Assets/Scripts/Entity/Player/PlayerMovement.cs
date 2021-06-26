using UnityEngine;
using Utils;

namespace Entity.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 7.5f;
    
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private EntityAnimator animator;

        public bool IsEnabled { get; set; } = true;

        private Vector2 _movement;

        public Direction Direction => EntityAnimator.GetDirectionIndex(_movement);

        private void Start()
        {
            rb ??= GetComponent<Rigidbody2D>();
            animator ??= GetComponent<EntityAnimator>();
        }

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _movement.Normalize();
            
            animator.Move(IsEnabled ? _movement : Vector2.zero);
        }

        private void FixedUpdate()
        {
            if (!IsEnabled) return;
            rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}
