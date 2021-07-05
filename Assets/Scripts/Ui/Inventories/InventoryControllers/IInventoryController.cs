using Inventory;
using Ui.Inventories.ItemSlot;

namespace Ui.Inventories.InventoryControllers
{
    public interface IInventoryController
    {
        IInventory GetInventory();
        void Init(IInventory inventory);
        void ExchangeItemStacks(IItemStackSlot from, IItemStackSlot to);
        void CrossInventoryExchange(IItemStackSlot from, IItemStackSlot to);
        void CrossControllerExchange(IItemStackSlot from, IItemStackSlot to);
        void ResetSlots();

        void UpdateSlots();
    }
}