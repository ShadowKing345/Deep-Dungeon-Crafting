using System;
using Project.Runtime.Entity.Combat;
using Project.Runtime.Items;
using Project.Runtime.Managers;
using UnityEngine;

namespace Project.Runtime.Inventory
{
    [Serializable]
    public class WeaponInventory : IInventory
    {
        [SerializeField] private ItemStack weapon = ItemStack.Empty;

        public ItemStack AddStackAtSlot(ItemStack stack, int index)
        {
            if (index != 0 || !(stack.Item is WeaponItem)) return stack;

            var result = weapon.Copy;
            weapon.Item = stack.Item;
            weapon.Amount = stack.Amount;

            if (((WeaponItem) weapon.Item).WeaponClass != null)
                OnWeaponChanged?.Invoke(((WeaponItem) weapon.Item).WeaponClass);

            return result;
        }

        public ItemStack GetStackAtSlot(int index)
        {
            return index == 0 ? weapon : ItemStack.Empty;
        }

        public ItemStack RemoveStackAtSlot(int index)
        {
            if (index != 0) return ItemStack.Empty;

            var result = weapon.Copy;
            weapon.Clear();

            OnWeaponChanged?.Invoke(GameManager.Instance.noWeaponClass);

            return result;
        }

        public ItemStack[] AddItemStacks(ItemStack[] stacks, bool combine = true)
        {
            var stack = stacks[0];

            if (combine && weapon.Item == stack.Item)
                stack.RemoveItem(stack.Amount - weapon.AddItem(stack.Item, stack.Amount));
            else if (weapon.IsEmpty)
                stack.RemoveItem(stack.Amount - weapon.AddItem(stack.Item, stack.Amount));

            return stacks;
        }

        public ItemStack[] GetItemStacks()
        {
            return new[] {weapon};
        }

        public ItemStack[] RemoveItemStacks(ItemStack[] stacks)
        {
            throw new NotImplementedException();
        }

        public ItemStack[] GetAndClearItemStacks()
        {
            var result = GetItemStacks();
            ResetInventory();
            return result;
        }

        public bool Contains(ItemStack stack)
        {
            return stack.Item == weapon.Item;
        }

        public bool ContainsExact(ItemStack stack)
        {
            return Contains(stack) && stack.Amount <= weapon.Amount;
        }

        public bool CanFitInSlot(ItemStack stack, int index)
        {
            return index == 0 && stack.Item is WeaponItem;
        }

        public bool CanFit(ItemStack stack)
        {
            return stack.Item is WeaponItem;
        }

        public void ResetInventory()
        {
            weapon.Clear();
        }

        public int Size => 1;

        public void SwapSlots(int fromIndex, int toIndex, out ItemStack fromStack, out ItemStack toStack)
        {
            fromStack = ItemStack.Empty;
            toStack = ItemStack.Empty;
        }

        public void SplitStack(int index, int amount)
        {
        }

        public void CombineStacks(int fromIndex, int toIndex)
        {
        }

        public event Action<WeaponClass> OnWeaponChanged;

        public ItemStack[] AddItemStacks(ItemStack[] stacks)
        {
            if (stacks.Length > 0)
                AddStackAtSlot(stacks[0], 0);

            return stacks;
        }
    }
}