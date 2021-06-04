using System;
using Items;
using Managers;
using UnityEngine;
using Weapons;

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

            OnWeaponChanged?.Invoke(((WeaponItem) weapon.Item).weaponClass);
            
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
            if (stacks.Length <= 0) return stacks;

            stacks[0] = AddStackAtSlot(stacks[0], 0);
            return stacks;
        }

        public ItemStack[] GetItemStacks() => new[] {weapon};

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

        public void ResetInventory() => weapon.Clear();

        public void SwapSlots(int fromIndex, int toIndex, out ItemStack stack,out ItemStack toStack)
        {
            throw new NotImplementedException();
        }
    }
}