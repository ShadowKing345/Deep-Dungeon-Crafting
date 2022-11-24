using Project.Runtime.Items;

namespace Project.Runtime.Inventory
{
    public interface IInventory
    {
        #region ItemStack Management

        ItemStack AddStackAtSlot(ItemStack stack, int index);
        ItemStack GetStackAtSlot(int index);
        ItemStack RemoveStackAtSlot(int index);

        #endregion

        #region ItemStacks Management

        ItemStack[] AddItemStacks(ItemStack[] stacks, bool combine = true);
        ItemStack[] GetItemStacks();

        ItemStack[] RemoveItemStacks(ItemStack[] stacks);

        // Clears Everything.
        ItemStack[] GetAndClearItemStacks();

        #endregion

        #region Utils

        bool Contains(ItemStack stack);
        bool ContainsExact(ItemStack stack);
        bool CanFitInSlot(ItemStack stack, int index);
        bool CanFit(ItemStack stack);
        void ResetInventory();

        int Size { get; }

        #endregion

        #region Item Stack Movement

        void SwapSlots(int fromIndex, int toIndex, out ItemStack fromStack, out ItemStack toStack);
        void SplitStack(int index, int amount);

        void CombineStacks(int fromIndex, int toIndex);

        #endregion
    }
}