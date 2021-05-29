using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class ItemStack
    {
        [SerializeField] private Item item;
        [SerializeField] private int amount;
        [SerializeField] private int maxStackSize = 999;

        public Item GetItem => item;
        public int GetAmount => amount;
        public int MaxStackSize
        {
            get => maxStackSize;
            set => maxStackSize = Math.Max(999, value);
        }

        public int AddItem(Item item, int amount)
        {
            if (this.item == null)
            {
                this.item = item;
            }
            if (item != this.item) return amount;

            this.amount += amount;
            if (this.amount > maxStackSize)
            {
                int remainder = this.amount - maxStackSize;
                this.amount = maxStackSize;
                return remainder;
            }

            return 0;
        }

        public ItemStack AddItem(ItemStack stack)
        {
            stack.amount -= stack.amount - AddItem(stack.item, stack.amount);

            return stack.IsEmpty ? empty : stack;
        }

        public int RemoveItem(int amount)
        {
            if (IsEmpty) return 0;

            this.amount -= amount;
            if (this.amount <= 0)
            {
                int remainder = -this.amount;
                Clear();
                return remainder;
            }
            
            return 0;
        }
        
        public bool IsEmpty => this == empty || item == null || amount <= 0;
        public static ItemStack empty = new ItemStack();

        private void Clear()
        {
            item = null;
            amount = 0;
        }
    }
}