using System;
using Combat;
using Items;
using Managers;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class WeaponInventory : IInventory
    {
        [SerializeField] private ItemStack weapon = ItemStack.Empty;

        public event Action<WeaponClass> OnWeaponChanged;

        public ItemStack AddStackAtSlot(ItemStack stack, int index)
        {
            if (index != 0 || !(stack.Item is WeaponItem)) return stack;

            ItemStack result = weapon.Copy;
            weapon.Item = stack.Item;
            weapon.Amount = stack.Amount;

            if (((WeaponItem) weapon.Item).WeaponClass != null)
                OnWeaponChanged?.Invoke(((WeaponItem) weapon.Item).WeaponClass);

            return result;
        }

        public ItemStack GetStackAtSlot(int index) => index == 0 ? weapon : ItemStack.Empty;

        public ItemStack RemoveStackAtSlot(int index)
        {
            if (index != 0) return ItemStack.Empty;
            
            ItemStack result = weapon.Copy;
            weapon.Clear();
            
            OnWeaponChanged?.Invoke(GameManager.instance.noWeaponClass);

            return result;
        }

        public ItemStack[] AddItemStacks(ItemStack[] stacks, bool combine = true)
        {
            ItemStack stack = stacks[0];
            
            if (combine && weapon.Item == stack.Item)
                stack.RemoveItem(stack.Amount - weapon.AddItem(stack.Item, stack.Amount));
            else if (weapon.IsEmpty)
                stack.RemoveItem(stack.Amount - weapon.AddItem(stack.Item, stack.Amount));
            
            return stacks;
        }

        public ItemStack[] AddItemStacks(ItemStack[] stacks)
        {
            if (stacks.Length > 0)
                AddStackAtSlot(stacks[0], 0);
            
            return stacks;
        }

        public ItemStack[] GetItemStacks() => new[] {weapon};
        public ItemStack[] RemoveItemStacks(ItemStack[] stacks)
        {
            throw new NotImplementedException();
        }

        public ItemStack[] GetAndClearItemStacks()
        {
            ItemStack[] result = GetItemStacks();
            ResetInventory();
            return result;
        }

        public bool Contains(ItemStack stack) => stack.Item == weapon.Item;

        public bool ContainsExact(ItemStack stack) => Contains(stack) && stack.Amount <= weapon.Amount;

        public bool CanFitInSlot(ItemStack stack, int index) => index == 0 && stack.Item is WeaponItem;

        public bool CanFit(ItemStack stack) => stack.Item is WeaponItem;

        public void ResetInventory() => weapon.Clear();
        public int Size => 1;

        public void SwapSlots(int fromIndex, int toIndex, out ItemStack fromStack,out ItemStack toStack) => throw new NotImplementedException();
        public void SplitStack(int index, int amount)
        {
            throw new NotImplementedException();
        }
    }
}