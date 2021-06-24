using Items;
using Ui.InventoryControllers;
using UnityEngine.EventSystems;

namespace Ui.ItemSlot
{
    public interface IItemStackSlot : IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler
    {
        void Init(int index, ItemStack stack, IInventoryController controller);
        int GetIndex();
        IInventoryController GetController();
        void SetItemStack(ItemStack stack);
        ItemStack GetItemStack();
        void UpdateUi();
        void ResetSlot();
    }
}