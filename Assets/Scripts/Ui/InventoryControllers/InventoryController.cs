using System;
using System.Collections.Generic;
using Inventory;
using Items;
using UnityEngine;

namespace Ui.InventoryControllers
{
    public class InventoryController : MonoBehaviour, IInventoryController
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject container;
        private IInventory _inventory;
        public IInventory GetInventory() => _inventory;
        private readonly List<IItemStackSlot> _slots = new List<IItemStackSlot>();

        public event Action OnUpdateUi;

        public void Init(IInventory inventory)
        {
            _inventory = inventory;
            ItemStack[] stacks = inventory.GetItemStacks();
            
            _slots.Clear();
            for (int i = 0; i < stacks.Length; i++)
            {
                ItemStack stack = stacks[i];
                GameObject slotObj = Instantiate(slotPrefab, container.transform);
                IItemStackSlot slot = slotObj.GetComponent<IItemStackSlot>();
                slot.Init(i, stack, this);
                OnUpdateUi += slot.UpdateUi;
                _slots.Add(slot);
            }
        }

        public void ExchangeItemStacks(int from, int to)
        {
            if (_inventory.GetStackAtSlot(to).IsEmpty)
                _inventory.AddStackAtSlot(_inventory.GetStackAtSlot(from), to);
            else
                _inventory.SwapSlots(from, to, out _, out _);

            _slots[to].UpdateUi();
            _slots[from].UpdateUi();
        }

        public void CrossControllerExchange(IItemStackSlot from, IItemStackSlot to)
        {
            if (!_inventory.CanFit(from.GetItemStack())) return;
            
            IInventory fromInventory = from.GetController().GetInventory();
            
            ItemStack fromStack = fromInventory.RemoveStackAtSlot(from.GetIndex());
            
            if (to.GetItemStack().IsEmpty)
            {
                _inventory.AddStackAtSlot(fromStack, to.GetIndex());
            }
            else
            {
                ItemStack toStack = _inventory.RemoveStackAtSlot(to.GetIndex());

                fromInventory.AddItemStacks(new[] {_inventory.AddStackAtSlot(fromStack, to.GetIndex())});
                _inventory.AddItemStacks(new[] {fromInventory.AddStackAtSlot(toStack, from.GetIndex())});
            }

            to.GetController().UpdateSlots();
            from.GetController().UpdateSlots();
        }

        public void ResetSlots() => _slots.Clear();

        public void UpdateSlots() => OnUpdateUi?.Invoke();
    }
}