using System;
using Inventory;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Entity
{
    [RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
    public class ItemEntity : MonoBehaviour
    {
        [SerializeField] private ItemStack stack;

        [SerializeField] private SpriteRenderer spriteRenderer;

        private void OnTriggerEnter2D(Collider2D other)
        {
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