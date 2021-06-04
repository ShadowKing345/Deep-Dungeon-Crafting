using Inventory;
using Items;
using UnityEngine;

namespace Ui.InventoryControllers
{
    public class WeaponInventoryController: MonoBehaviour, IInventoryController
    {
        private IItemStackSlot _slot;
        private WeaponInventory _inventory;
        public IInventory GetInventory() => _inventory;

        private void Awake()
        {
            _slot ??= GetComponentInChildren<IItemStackSlot>();
        }

        public void Init(IInventory inventory)
        {
            if (_slot == null) return;

            if (inventory is WeaponInventory weaponInventory)
            {
                _inventory = weaponInventory;
                _slot.Init(0, _inventory.GetStackAtSlot(0), this);
                _slot.UpdateUi();
            }
        }

        public void ExchangeItemStacks(int from, int to)
        {
            
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
            throw new System.NotImplementedException();
        }
    }
}