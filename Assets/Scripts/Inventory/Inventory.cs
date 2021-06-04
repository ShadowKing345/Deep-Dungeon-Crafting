using System;
using Items;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class Inventory : IInventory
    {
        public int capacity = 40;
        [SerializeField] private ItemStack[] itemStacks = new ItemStack[30];

        public ItemStack AddStackAtSlot(ItemStack stack, int index)
        {
            if (index >= capacity) return stack;

            ItemStack inventoryStack = itemStacks[index];

            return inventoryStack.AddItem(stack);
        }

        public ItemStack GetStackAtSlot(int index)
        {
            return itemStacks[index];
        }

        public ItemStack RemoveStackAtSlot(int index)
        {
            ItemStack result = itemStacks[index].Copy;
            itemStacks[index].Clear();
            return result;
        }

        public ItemStack[] AddItemStacks(ItemStack[] stacks)
        {
            foreach (var stack in stacks)
            {
                foreach (ItemStack inventoryStack in itemStacks)
                {
                    stack.RemoveItem(stack.Amount - inventoryStack.AddItem(stack.Item, stack.Amount));
                    if (stack.IsEmpty) break;
                }
            }

            return stacks;
        }

        public ItemStack[] GetItemStacks()
        {
            return itemStacks;
        }

        public ItemStack[] GetAndClearItemStacks()
        {
            ItemStack[] result = GetItemStacks();
            ResetInventory();
            return result;
        }

        public bool Contains(ItemStack stack)
        {
            throw new NotImplementedException();
        }

        public bool ContainsExact(ItemStack stack)
        {
            throw new NotImplementedException();
        }

        public void ResetInventory()
        {
            itemStacks = new ItemStack[capacity];
            for (int i = 0; i < capacity; i++)
            {
                itemStacks[i] = ItemStack.Empty;
            }
        }

        public void SwapSlots(int fromIndex, int toIndex, out ItemStack fromStack, out ItemStack toStack)
        {
            toStack = RemoveStackAtSlot(fromIndex);
            fromStack = RemoveStackAtSlot(toIndex);

            AddStackAtSlot(toStack.Copy, toIndex);
            AddStackAtSlot(fromStack.Copy, fromIndex);
        }
    }
}