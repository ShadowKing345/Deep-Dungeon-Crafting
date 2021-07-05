using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Entity.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 7.5f;
        [Space]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private EntityAnimator animator;

        public bool IsEnabled { get; set; } = true;

        private Vector2 _movement = Vector2.zero;
        public Direction Direction => EntityAnimator.GetDirectionIndex(_movement);

        public void Move(InputAction.CallbackContext context) => _movement = context.ReadValue<Vector2>();

        private void Update() => animator.Move(IsEnabled ? _movement : Vector2.zero);

        private void FixedUpdate()
        {
            if (!IsEnabled) return;
            rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
            // _movement = Vector2.zero;
        }
    }
}
