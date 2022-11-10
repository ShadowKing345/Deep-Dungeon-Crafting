using System;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entity.Player
{
    public class PlayerInputManager : MonoBehaviour, InputManager.IPlayerActions
    {
        public Player player;

        private InputManager inputManager;
        
        public float aoeSize;
        public Vector2 aoeOffset;
        
        private void OnEnable()
        {
            if (inputManager == null)
            {
                inputManager = new InputManager();
                inputManager.Player.SetCallbacks(this);
            }
            inputManager.Player.Enable();
        }

        private void OnDisable() => inputManager.Player.Disable();

        private void SelectInteractable()
        {
            var currentPos = (Vector2) transform.position + aoeOffset;
            var hits = new Collider2D[] { };
            Physics2D.OverlapCircleNonAlloc(currentPos, aoeSize, hits);

            IInteractable closest = null;
            var smallestPos = Vector2.positiveInfinity;

            foreach(var hit in hits)
            {
                if (hit.gameObject == gameObject) continue;
                if (!hit.TryGetComponent(out IInteractable interactable)) continue;
                
                if (Vector2.Distance(currentPos, hit.transform.position) >= Vector2.Distance(currentPos, smallestPos)) continue;

                smallestPos = hit.transform.position;
                closest = interactable;
            }

            closest?.Interact(gameObject);
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            player.playerMovementManager.Move(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnAbility1(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnAbility2(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnAbility3(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
    }
}