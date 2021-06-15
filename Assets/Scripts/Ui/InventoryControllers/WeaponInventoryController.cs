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
        
        private void Awake() => ResetSlots();

        public void Init(IInventory inventory)
        {
            if (_slot == null) return;
            if (!(inventory is WeaponInventory weaponInventory)) return;
            
            _inventory = weaponInventory;
            _slot.Init(0, _inventory.GetStackAtSlot(0), this);
            _slot.UpdateUi();
        }

        public void ExchangeItemStacks(int from, int to) { }

        public void CrossControllerExchange(IItemStackSlot from, IItemStackSlot to)
        {
            if (!_inventory.CanFit(from.GetItemStack())) return;
            
            IInventory fromInventory = from.GetController().GetInventory();

            ItemStack fromStack = fromInventory.RemoveStackAtSlot(from.GetIndex());
            
            if (to.GetItemStack().IsEmpty)
            {
                fromInventory.AddItemStacks(new []{_inventory.AddStackAtSlot(fromStack, to.GetIndex())});
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

        public void ResetSlots() => _slot ??= GetComponentInChildren<IItemStackSlot>();

        public void UpdateSlots() => _slot.UpdateUi();
    }
}