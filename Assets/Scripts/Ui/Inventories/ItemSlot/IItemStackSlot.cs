using Inventory;
using Items;
using Pathfinding;
using Ui.Inventories.InventoryControllers;
using UnityEngine.EventSystems;

namespace Ui.Inventories.ItemSlot
{
    public interface IItemStackSlot : IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler
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