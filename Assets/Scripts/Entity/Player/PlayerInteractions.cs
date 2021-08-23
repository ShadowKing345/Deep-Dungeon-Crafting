using Interfaces;
using Managers;
using UnityEngine;

namespace Entity.Player
{
    public class PlayerInteractions : MonoBehaviour
    {
        private InputManager _inputManager;
        
        public float aoeSize;
        public Vector2 aoeOffset;
        public new Collider2D collider2D;

        private void OnEnable()
        {
            _inputManager ??= new InputManager();

            _inputManager.Player.Interact.canceled += _ =>
            {
                SelectInteractable();
            };
            _inputManager.Player.Enable();
        }

        private void OnDisable() => _inputManager.Player.Disable();

        private void SelectInteractable()
        {
            Vector2 currentPos = (Vector2) transform.position + aoeOffset;
            Collider2D[] hits = Physics2D.OverlapCircleAll(currentPos, aoeSize);

            IInteractable closest = null;
            Vector2 smallestPos = Vector2.positiveInfinity;

            foreach(Collider2D hit in hits)
            {
                if (hit.gameObject == gameObject) continue;
                if (!hit.TryGetComponent(out IInteractable interactable)) continue;
                
                if (Vector2.Distance(currentPos, hit.transform.position) >= Vector2.Distance(currentPos, smallestPos)) continue;

                smallestPos = hit.transform.position;
                closest = interactable;
            }

            closest?.Interact(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            if (aoeSize <= 0) return;
            
            Gizmos.DrawWireSphere((Vector2) transform.position + aoeOffset, aoeSize);
        }
    }
}