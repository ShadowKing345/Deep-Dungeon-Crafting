using System;
using Interfaces;
using Managers;
using UnityEngine;
using Utils;

namespace Entity.Player
{
    public class PlayerInteractions : MonoBehaviour
    {
        private InputHandler inputHandler;
        private WindowManager windowManager;
        
        public Vector2 aoeSize;
        public Vector2 aoeOffset;

        private void Start()
        {
            inputHandler ??= InputHandler.instance;
            windowManager ??= WindowManager.instance;
        }

        private void Update()
        {
            if (inputHandler.GetKeyDown(InputHandler.KeyValue.Interact)) SelectInteractable();
            if (inputHandler.GetKeyDown(InputHandler.KeyValue.OpenInventory)) windowManager.ToggleUiElement(WindowManager.UiElementReference.PlayerMenu);
            if (inputHandler.GetKeyDown(InputHandler.KeyValue.OpenCraftingMenu)) windowManager.ToggleUiElement(WindowManager.UiElementReference.CraftingMenu);
            if (inputHandler.GetKeyDown(InputHandler.KeyValue.PauseResumeGame)) windowManager.ToggleUiElement(WindowManager.UiElementReference.PauseMenu);
        }

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

        private void OnDrawGizmos()
        {
            if (aoeSize.magnitude <= 0) return;
            
            Gizmos.DrawWireCube((Vector2) transform.position + aoeOffset, aoeSize);
        }
    }
}