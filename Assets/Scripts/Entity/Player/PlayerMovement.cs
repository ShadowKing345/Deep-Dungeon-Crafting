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
        private InputManager inputManager;
        
        [SerializeField] private float moveSpeed = 7.5f;
        [Space]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private EntityAnimator animator;

        public bool IsEnabled { get; set; } = true;

        private Vector2 _movement = Vector2.zero;
        public Direction Direction => EntityAnimator.GetDirectionIndex(_movement);
        
        private void OnEnable()
        {
            inputManager ??= new InputManager();

            inputManager.Player.Move.performed += ctx => _movement = ctx.ReadValue<Vector2>();
            inputManager.Player.Move.canceled += ctx => _movement = ctx.ReadValue<Vector2>();
            
            inputManager.Player.Enable();
        }

        private void OnDisable() => inputManager.Player.Disable();

        private void Update() => animator.Move(IsEnabled ? _movement : Vector2.zero);

        private void FixedUpdate()
        {
            if (!IsEnabled) return;
            rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
        }

        private void Reset() => inputManager ??= new InputManager();
    }
}
