using UnityEngine;

namespace Project.Runtime.Entity.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : EntityAnimator
    {
        [SerializeField] private float moveSpeed = 7.5f;

        // ReSharper disable once NotAccessedField.Global
        public Player player;
        private Vector2 movementDirection;

        private Rigidbody2D rb;

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody2D>();
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

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (movementDirection.magnitude == 0) return;
            rb.MovePosition(rb.position + movementDirection * (moveSpeed * Time.fixedDeltaTime));
        }

        public void Move(Vector2 direction)
        {
            movementDirection = direction;
        }
    }
}