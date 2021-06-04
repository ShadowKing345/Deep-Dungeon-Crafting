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
            IInventory fromInventory = from.GetController().GetInventory();
            
            if (to.GetItemStack().IsEmpty)
            {
                ItemStack fromStack = fromInventory.RemoveStackAtSlot(from.GetIndex());

                _inventory.AddStackAtSlot(fromStack, to.GetIndex());
            }
            else
            {
                ItemStack fromStack = fromInventory.RemoveStackAtSlot(from.GetIndex());
                ItemStack toStack = _inventory.RemoveStackAtSlot(to.GetIndex());

                _inventory.AddStackAtSlot(fromStack, to.GetIndex());
                fromInventory.AddStackAtSlot(toStack, from.GetIndex());
            }

            from.UpdateUi();
            to.UpdateUi();
        }

        public void ResetSlots()
        {
            _slots.Clear();
        }
    }
}