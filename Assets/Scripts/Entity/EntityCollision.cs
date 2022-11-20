using UnityEngine;

namespace Entity
{
    public class EntityCollision : MonoBehaviour
    {
        [SerializeField] private Collider2D entityCollider;
        [SerializeField] private Collider2D entityBlockerCollider;

        private void OnEnable()
        {
            Physics2D.IgnoreCollision(entityCollider, entityBlockerCollider, true);
        }
    }
}