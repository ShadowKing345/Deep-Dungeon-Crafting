using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class ArmorInventory : IInventory
    {
        [SerializeField] private ItemStack head; 
        [SerializeField] private ItemStack body; 
        [SerializeField] private ItemStack legs; 
        [SerializeField] private ItemStack earring; 
        [SerializeField] private ItemStack bracelet; 
        [SerializeField] private ItemStack ring;

        public ItemStack AddStackAtSlot(ItemStack stack, int index)
        {
            if (!Enum.IsDefined(typeof(ArmorType), index)) return stack;
            if (!(stack.Item is ArmorItem)) return stack;

            AddItem((ArmorType) index switch
            {
                ArmorType.Head => head,
                ArmorType.Body => body,
                ArmorType.Legs => legs,
                ArmorType.Earring => earring,
                ArmorType.Bracelet => bracelet,
                ArmorType.Ring => ring,
                _ => null
            }, stack);

            return stack;
        }

        private void AddItem(ItemStack input, ItemStack output)
        {
            if (input == null) return;
            ItemStack hold = input.Copy;
            
            input.Item = output.Item;
            input.Amount = output.Amount;

            output.Item = hold.Item;
            output.Amount = hold.Amount;
        }

        public ItemStack GetStackAtSlot(int index) => 
            (ArmorType) index switch
            {
                ArmorType.Head => head,
                ArmorType.Body => body,
                ArmorType.Legs => legs,
                ArmorType.Earring => earring,
                ArmorType.Bracelet => bracelet,
                ArmorType.Ring => ring,
                _ => ItemStack.Empty
            };
        
        public ItemStack RemoveStackAtSlot(int index)
        {
            if (!Enum.IsDefined(typeof(ArmorType), index)) return ItemStack.Empty;
            
            ItemStack stack = GetStackAtSlot(index);
            ItemStack result = stack.Copy;
            stack.Clear();

            return result;
        }
        
        public ItemStack[] AddItemStacks(ItemStack[] stacks, bool _ = false) {
            for (int i = 0; i < 6; i++) AddStackAtSlot(stacks[i], i);
            return stacks;
        }

        public ItemStack[] GetItemStacks() => new [] {head, body, legs, earring, bracelet, ring};
        public ItemStack[] RemoveItemStacks(ItemStack[] stacks)
        {
            throw new NotImplementedException();
        }

        public ItemStack[] GetAndClearItemStacks()
        {
            List<ItemStack> result = new List<ItemStack>();
            
            foreach (ItemStack stack in GetItemStacks())
            {
                result.Add(stack.Copy);
                stack.Clear();
            }

            return result.ToArray();
        }

        public bool Contains(ItemStack stack)
        {
            if (!(stack.Item is ArmorItem item)) return false;
            return GetStackAtSlot((int) item.type).Item == item;
        }

        public bool ContainsExact(ItemStack stack) => Contains(stack);

        public bool CanFitInSlot(ItemStack stack, int index) => Enum.IsDefined(typeof(ArmorType), index) && stack.Item is ArmorItem item && item.type == (ArmorType) index;

        public bool CanFit(ItemStack stack) => true;

        public void ResetInventory() { foreach (ItemStack stack in GetItemStacks()) stack.Clear(); }
        public int Size => 6;

        public void SwapSlots(int fromIndex, int toIndex, out ItemStack fromStack, out ItemStack toStack) { fromStack = ItemStack.Empty; toStack = ItemStack.Empty; }
        public void SplitStack(int index, int amount) { }
        public void CombineStacks(int fromIndex, int toIndex) { }
    }
}