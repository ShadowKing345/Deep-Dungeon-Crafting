using Project.Runtime.Inventory;
using Project.Runtime.Items;
using Project.Runtime.Ui.Inventories.InventoryControllers;

namespace Project.Runtime.Ui.Inventories.ItemSlot
{
    public interface IItemStackSlot
    {
        int Id { get; set; }
        ItemStack ItemStack { get; set; }
        IInventoryController Controller { get; set; }
        int InventoryIndex { get; set; }
        IInventory Inventory { get; set; }

        void Init(int id, ItemStack stack, IInventoryController controller, int inventoryIndex, IInventory inventory);
        void UpdateUi();
        void ResetSlot();
        bool CanFit(ItemStack stack);
    }
}