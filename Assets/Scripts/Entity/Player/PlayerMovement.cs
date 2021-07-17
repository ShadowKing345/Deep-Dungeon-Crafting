using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

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
        
        private Vector2 _movement = Vector2.zero;

        private void OnEnable()
        {
            inputManager ??= new InputManager();

            inputManager.Player.Move.performed += ctx => _movement = ctx.ReadValue<Vector2>();
            inputManager.Player.Move.canceled += ctx => _movement = ctx.ReadValue<Vector2>();
            
            inputManager.Player.Enable();
        }

        private void OnDisable() => inputManager.Player.Disable();

        private void Update() => animator.Move(_movement);

        private void FixedUpdate() => rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
    }
}
