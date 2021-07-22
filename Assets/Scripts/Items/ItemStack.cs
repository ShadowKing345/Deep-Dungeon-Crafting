using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class ItemStack
    {
        [SerializeField] private Item item;
        [SerializeField] private int amount;

        public Item Item
        {
            get => item;
            set => item = value;
        }

        public int Amount
        {
            get => amount;
            set => amount = value;
        }

        public int MaxStackSize => item == null ? 0: item.MaxStackSize;

        public int AddItem(Item item, int amount)
        {
            if (this.item == null) Item = item;
            if (item != this.item) return amount;

            this.amount += amount;
            if (this.amount <= item.MaxStackSize) return 0;
            
            int remainder = this.amount - item.MaxStackSize;
            this.amount = item.MaxStackSize;
            return remainder;

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
            
            if (this.amount > 0) return amount;
            
            int remainder = -this.amount;
            Clear();
            return remainder;

        }

        public ItemStack RemoveItem(ItemStack stack)
        {
            if (stack.item != item) return Empty;

            int remainder = RemoveItem(stack.Amount);
            return remainder <= 0 ? Empty : new ItemStack{Item = stack.item, amount = remainder};
        }
        
        public bool IsEmpty => item == null || amount <= 0;
        public ItemStack Copy => new ItemStack { item = item, amount = amount};
        public static readonly ItemStack Empty = new ItemStack();

        public void Clear()
        {
            item = null;
            amount = 0;
        }
    }
}