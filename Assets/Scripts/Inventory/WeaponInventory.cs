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

            if (((WeaponItem) weapon.Item).weaponClass != null)
            {
                OnWeaponChanged?.Invoke(((WeaponItem) weapon.Item).weaponClass);
            }

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

        public ItemStack[] AddItemStacks(ItemStack[] stacks)
        {
            if (stacks.Length > 0)
                AddStackAtSlot(stacks[0], 0);
            
            return stacks;
        }

        public ItemStack[] GetItemStacks() => new[] {weapon};

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

        public void SwapSlots(int fromIndex, int toIndex, out ItemStack fromStack,out ItemStack toStack) => throw new NotImplementedException();
    }
}