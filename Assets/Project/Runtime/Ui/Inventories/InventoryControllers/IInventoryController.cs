using Project.Runtime.Inventory;
using Project.Runtime.Ui.Inventories.ItemSlot;

namespace Project.Runtime.Ui.Inventories.InventoryControllers
{
    public interface IInventoryController
    {
        IInventory GetInventory();
        void Init(IInventory inventory);
        void ExchangeItemStacks(IItemStackSlot from, IItemStackSlot to);
        void CrossInventoryExchange(IItemStackSlot from, IItemStackSlot to);
        void CrossControllerExchange(IItemStackSlot from, IItemStackSlot to);
        void SplitSlot(IItemStackSlot slot);
        void ClearSlot(IItemStackSlot slot);
        void ResetSlots();

        void UpdateSlots();
    }
}