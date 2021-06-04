using Items;

namespace Inventory
{
    public interface IInventoryController
    {
        IInventory GetInventory();
        void Init(IInventory inventory);
        void ExchangeItemStacks(int from, int to);
        void CrossControllerExchange(IItemStackSlot from, IItemStackSlot to);
        void ResetSlots();
    }
}