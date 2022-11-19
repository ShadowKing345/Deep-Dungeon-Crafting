using UnityEngine;
using Utils;

namespace Entity.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : EntityAnimator
    {
        [SerializeField] private float moveSpeed = 7.5f;

        private Rigidbody2D rb;
        private Vector2 movementDirection;

        // ReSharper disable once NotAccessedField.Global
        public Player player;

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody2D>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (movementDirection.magnitude == 0) return;
            rb.MovePosition(rb.position + movementDirection * (moveSpeed * Time.fixedDeltaTime));
        }

        protected override void Update()
        {
            if (movementDirection.magnitude > 0)
            {
                currentDirection = GetDirectionIndex(movementDirection);
                state = State.Moving;
            }
            else
            {
                state = State.Idle;
            }
            
            base.Update();
        }

        public void Move(Vector2 direction)
        {
            movementDirection = direction;
        }
    }
}