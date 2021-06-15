using Interfaces;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerInteractions : MonoBehaviour
    {
        public Vector2 aoeSize;
        public Vector2 aoeOffset;
        
        private void Update()
        {
            if (InputHandler.instance.GetKeyDown(InputHandler.KeyValue.Interact)) SelectInteractable();
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