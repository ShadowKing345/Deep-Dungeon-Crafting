using Items;

namespace Inventory
{
    public interface IEntityInventoryController
    {
        IInventory IndexMapping(int index, out int newIndex);
        IInventory InventoryMapping(int mappingIndex);
        
        #region ItemStack Management

        ItemStack AddStackAtSlot(ItemStack stack, int index);
        ItemStack GetStackAtSlot(int index);
        ItemStack RemoveStackAtSlot(int index);

        #endregion

        #region ItemStacks Management

        ItemStack[] AddItemStacks(ItemStack[] stacks, bool combine = true, int mappingIndex = 0);
        ItemStack[] GetItemStacks(int mappingIndex = 0);
        ItemStack[] RemoveItemStacks(ItemStack[] stacks, int mappingIndex = 0);
        // Clears Everything.
        ItemStack[] GetAndClearItemStacks(int mappingIndex = 0);
        
        #endregion

        #region Utils

        bool Contains(ItemStack stack, int mappingIndex = 0);
        bool ContainsExact(ItemStack stack, int mappingIndex = 0);
        bool CanFitInSlot(ItemStack stack, int index);
        bool CanFit(ItemStack stack, int mappingIndex = 0);
        void ResetInventory(int mappingIndex = 0);

        #endregion
    }
}