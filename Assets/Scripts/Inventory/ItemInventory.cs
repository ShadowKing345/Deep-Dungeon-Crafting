using System;
using System.Linq;
using Items;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class ItemInventory : IInventory
    {
        [SerializeField] private ItemStack[] itemStacks = new ItemStack[99];

        public ItemStack AddStackAtSlot(ItemStack stack, int index) => index >= itemStacks.Length ? stack : itemStacks[index].AddItem(stack);

        public ItemStack GetStackAtSlot(int index) => itemStacks[index];

        public ItemStack RemoveStackAtSlot(int index)
        {
            ItemStack result = itemStacks[index].Copy;
            itemStacks[index].Clear();
            return result;
        }

        public ItemStack[] AddItemStacks(ItemStack[] stacks, bool combine = true)
        {
            foreach (var stack in stacks)
            {
                if (stack.IsEmpty) continue;
                
                foreach (ItemStack inventoryStack in itemStacks)
                {
                    if (combine && inventoryStack.Item == stack.Item)
                        stack.RemoveItem(stack.Amount - inventoryStack.AddItem(stack.Item, stack.Amount));
                    else if (inventoryStack.IsEmpty)
                        stack.RemoveItem(stack.Amount - inventoryStack.AddItem(stack.Item, stack.Amount));

                    if (stack.IsEmpty) break;
                }
            }

            return stacks;
        }

        public ItemStack[] RemoveItemStacks(ItemStack[] stacks)
        {
            foreach (ItemStack stack in stacks)
            {
                foreach (ItemStack inventoryStack in itemStacks)
                {
                    if (inventoryStack.Item == stack.Item)
                        stack.RemoveItem(inventoryStack.RemoveItem(stack.Amount));

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

        public bool Contains(ItemStack stack) => itemStacks.FirstOrDefault(s => s.Item == stack.Item) != null;

        public bool ContainsExact(ItemStack stack) => itemStacks.FirstOrDefault(s => s.Item == stack.Item && s.Amount >= stack.Amount) != null;

        public bool CanFitInSlot(ItemStack stack, int index)
        {
            ItemStack itemStack = itemStacks[index];
            if (itemStack.IsEmpty) return true;
            return stack.Item == itemStack.Item;
        }

        public bool CanFit(ItemStack stack)
        {
            for (int i = 0; i < itemStacks.Length; i++)
                if (CanFitInSlot(stack, i))
                    return true;
            
            return false;
        }

        public void ResetInventory()
        {
            foreach (var stack in itemStacks)
                stack.Clear();
        }

        public int Size => itemStacks.Length;

        public void SwapSlots(int fromIndex, int toIndex, out ItemStack fromStack, out ItemStack toStack)
        {
            fromStack = RemoveStackAtSlot(fromIndex);

            if (itemStacks[toIndex].IsEmpty)
            {
                AddStackAtSlot(fromStack.Copy, toIndex);
                toStack = ItemStack.Empty;
                return;
            }
            
            toStack = RemoveStackAtSlot(toIndex);

            AddStackAtSlot(toStack.Copy, fromIndex);
            AddStackAtSlot(fromStack.Copy, toIndex);
        }

        public void SplitStack(int index, int amount)
        {
            ItemStack stack = itemStacks[index].Copy;
            stack.Amount = itemStacks[index].RemoveItem(amount);

            AddItemStacks(new[] {stack}, false);
        }

        public void CombineStacks(int fromIndex, int toIndex) => itemStacks[toIndex].AddItem(itemStacks[fromIndex]);
    }
}