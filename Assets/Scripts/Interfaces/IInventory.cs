using Items;

namespace Interfaces
{
    public interface IInventory
    {
        ItemStack AddStackAtSlot(ItemStack stack, int index);
        ItemStack GetStackAtSlot(int index);
        ItemStack RemoveStackAtSlot(int number, int index);

        ItemStack[] AddItemStacks(ItemStack[] stacks);
        ItemStack[] GetItemStacks();
        ItemStack[] GetAndClearItemStacks();

        void ResetInventory();
    }
}