using Inventory;
using Items;
using Ui.ItemSlot;

namespace Ui.InventoryControllers
{
    public interface IInventoryController
    {
        IInventory GetInventory();
        void Init(IInventory inventory);
        void ExchangeItemStacks(int from, int to);
        void CrossControllerExchange(IItemStackSlot from, IItemStackSlot to);
        void ResetSlots();

        void UpdateSlots();
    }
}