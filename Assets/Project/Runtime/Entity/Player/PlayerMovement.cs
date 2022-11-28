using UnityEngine;

namespace Project.Runtime.Entity.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : EntityAnimator
    {
        public PlayerEntity playerEntity;
        private float movementSpeed = 7.5f;
        private Vector2 movementDirection;

        private Rigidbody2D rb;

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody2D>();
        }

        protected override void Start()
        {
            if (playerEntity == null) return;

            entityData = playerEntity.Data;
            movementSpeed = entityData.MovementSpeed;

            base.Start();
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
            rb.MovePosition(rb.position + movementDirection * (movementSpeed * Time.fixedDeltaTime));
        }

        public void Move(Vector2 direction)
        {
            movementDirection = direction;
        }
    }
}