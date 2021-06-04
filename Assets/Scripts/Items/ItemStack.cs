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

        public Item Item
        {
            get => item;
            set => item = value;
        }

        public int Amount
        {
            get => amount;
            set => amount = Mathf.Clamp(value, 0, maxStackSize);
        }

        public int MaxStackSize
        {
            get => maxStackSize;
            set => maxStackSize = Math.Min(999, value);
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

            if (stack.IsEmpty) stack.Clear();
            return stack;
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
        
        public bool IsEmpty => item == null || amount <= 0;
        public ItemStack Copy => new ItemStack { item = item, maxStackSize = maxStackSize, amount = amount};
        public static readonly ItemStack Empty = new ItemStack();

        public void Clear()
        {
            item = null;
            amount = 0;
        }
    }
}