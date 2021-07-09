using System;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Entity.Player
{
    public class PlayerInteractions : MonoBehaviour
    {
        private InputManager _inputManager;
        private UiManager _uiManager;
        
        public Vector2 aoeSize;
        public Vector2 aoeOffset;

        private void OnEnable()
        {
            _uiManager ??= UiManager.Instance;
            _inputManager ??= new InputManager();

            _inputManager.Player.Interact.canceled += _ => SelectInteractable();
            _inputManager.Player.Enable();
        }

        private void OnDisable() => _inputManager.Player.Disable();

        private void SelectInteractable()
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll((Vector2) transform.position + aoeOffset, aoeSize, 0);

                // Todo Proximity filter.
            
            foreach(Collider2D collider2D1 in hits)
            {
                IInteractable interactable = collider2D1.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(gameObject);
                }
            }
        }
    }
}