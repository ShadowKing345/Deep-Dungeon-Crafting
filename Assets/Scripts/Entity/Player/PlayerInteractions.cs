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
        private WindowManager windowManager;
        
        public Vector2 aoeSize;
        public Vector2 aoeOffset;

        private void Start()
        {
            windowManager ??= WindowManager.instance;
        }

        private void Update()
        {
            // if (_inputManager.GetKeyDown(InputManager.KeyValue.OpenInventory)) windowManager.ToggleUiElement(WindowManager.UiElementReference.PlayerMenu);
            // if (_inputManager.GetKeyDown(InputManager.KeyValue.OpenCraftingMenu)) windowManager.ToggleUiElement(WindowManager.UiElementReference.CraftingMenu);
            // if (_inputManager.GetKeyDown(InputManager.KeyValue.PauseResumeGame)) windowManager.ToggleUiElement(WindowManager.UiElementReference.PauseMenu);
        }
        public void Interact(InputAction.CallbackContext ctx) => SelectInteractable();

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