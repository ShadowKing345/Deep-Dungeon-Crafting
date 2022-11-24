using System;
using UnityEngine;

namespace Project.Runtime.Items
{
    [Serializable]
    public class ItemStack
    {
        public static readonly ItemStack Empty = new();
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

        public int MaxStackSize => item == null ? 0 : item.MaxStackSize;

        public bool IsEmpty => item == null || amount <= 0;
        public ItemStack Copy => new() {item = item, amount = amount};

        public int AddItem(Item item, int amount)
        {
            if (this.item == null) Item = item;
            if (item != this.item) return amount;

            if (item == null) return amount;
            this.amount += amount;
            if (this.amount <= item.MaxStackSize) return 0;

            var remainder = this.amount - item.MaxStackSize;
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

            var remainder = -this.amount;
            Clear();
            return remainder;
        }

        public ItemStack RemoveItem(ItemStack stack)
        {
            if (stack.item != item) return Empty;

            var remainder = RemoveItem(stack.Amount);
            return remainder <= 0 ? Empty : new ItemStack {Item = stack.item, amount = remainder};
        }

        public void Clear()
        {
            item = null;
            amount = 0;
        }
    }
}