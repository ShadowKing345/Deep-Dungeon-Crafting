using System;
using System.Linq;
using Interfaces;
using Items;

namespace Inventory
{
    [Serializable]
    public class Inventory : IInventory
    {
        public int capacity = 30;
        public ItemStack[] itemStacks = new ItemStack[30];

        public ItemStack AddStackAtSlot(ItemStack stack, int index)
        {
            if (index >= capacity) return stack;

            ItemStack inventoryStack = itemStacks[index] ?? ItemStack.empty;

            return inventoryStack.AddItem(stack);
        }

        public ItemStack GetStackAtSlot(int index)
        {
            return itemStacks[index];
        }

        public ItemStack RemoveStackAtSlot(int number, int index)
        {
            ItemStack result = itemStacks[index];
            itemStacks[index] = ItemStack.empty;
            return result;
        }

        public ItemStack[] AddItemStacks(ItemStack[] stacks)
        {
            foreach (var stack in stacks)
            {
                foreach (ItemStack inventoryStack in itemStacks)
                {
                    stack.RemoveItem(stack.GetAmount - inventoryStack.AddItem(stack.GetItem, stack.GetAmount));
                    if (stack.IsEmpty) break;
                }
            }

            return stacks;
        }

        public ItemStack[] GetItemStacks()
        {
            return itemStacks.Where((stack, i) => !stack.IsEmpty).ToArray();
        }

        public ItemStack[] GetAndClearItemStacks()
        {
            ItemStack[] result = GetItemStacks();
            ResetInventory();
            return result;
        }

        public void ResetInventory()
        {
            itemStacks = new ItemStack[capacity];
            for (int i = 0; i < capacity; i++)
            {
                itemStacks[i] = ItemStack.empty;
            }
        }
    }
}