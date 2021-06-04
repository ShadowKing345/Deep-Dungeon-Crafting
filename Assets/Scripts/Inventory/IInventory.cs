using Items;

namespace Inventory
{
    public interface IInventory
    {
        ItemStack AddStackAtSlot(ItemStack stack, int index);
        ItemStack GetStackAtSlot(int index);
        ItemStack RemoveStackAtSlot(int index);

        ItemStack[] AddItemStacks(ItemStack[] stacks);
        ItemStack[] GetItemStacks();
        ItemStack[] GetAndClearItemStacks();

        bool Contains(ItemStack stack);
        bool ContainsExact(ItemStack stack);
        
        void ResetInventory();
        void SwapSlots(int fromIndex, int toIndex, out ItemStack fromStack, out ItemStack toStack);
    }
}