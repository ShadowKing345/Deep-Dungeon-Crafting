using Items;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public Inventory.Inventory inventory;
        public ItemStack[] stack;

        private void Start()
        {
            inventory.AddItemStacks(stack);
        }
    }
}