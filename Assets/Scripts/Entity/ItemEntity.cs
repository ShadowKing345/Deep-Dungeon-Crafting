using Inventory;
using Items;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
    public class ItemEntity : MonoBehaviour
    {
        [SerializeField] private ItemStack stack;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float awakeDelay = 0.5f;
        private float cooldown;

        private void Awake() => cooldown = Time.time + awakeDelay;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(cooldown > Time.time) return;

            if(other.gameObject == gameObject) return;

            if (!other.TryGetComponent(out IEntityInventoryController controller)) return;
            
            if(controller.AddItemStacks(new[] {stack})[0].IsEmpty)
                Destroy(gameObject);
        }
        
        public ItemStack ItemStack
        {
            get => stack;
            set
            {
                stack = value;
                spriteRenderer.sprite = value.IsEmpty ? null : value.Item.Icon;
            }
        }
    }
}