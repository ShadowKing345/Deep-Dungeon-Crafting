using UnityEngine;

namespace Entity.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementManager : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 7.5f;
        [Space]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private EntityAnimator animator;
        
        private Vector2 direction = Vector2.zero;
        
        private void Update()
        {
            if (animator)
            {
                animator.Move(direction);
            }
        }

        private void FixedUpdate()
        {
            if (rb)
            {
                rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
            }
        }

        public void Move(Vector2 direction)
        {
            this.direction = direction.normalized;
        }
    }
}
