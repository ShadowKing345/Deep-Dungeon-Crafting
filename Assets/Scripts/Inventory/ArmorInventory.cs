using Interfaces;
using Items;

namespace Inventory
{
    public class ArmorInventory : IInventory
    {
        public ItemStack AddStackAtSlot(ItemStack stack, int index)
        {
            throw new System.NotImplementedException();
        }

        public ItemStack GetStackAtSlot(int index)
        {
            throw new System.NotImplementedException();
        }

        public ItemStack RemoveStackAtSlot(int index)
        {
            throw new System.NotImplementedException();
        }

        public ItemStack[] AddItemStacks(ItemStack[] stacks)
        {
            throw new System.NotImplementedException();
        }

        public ItemStack[] GetItemStacks()
        {
            throw new System.NotImplementedException();
        }

        public ItemStack[] GetAndClearItemStacks()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(ItemStack stack)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsExact(ItemStack stack)
        {
            throw new System.NotImplementedException();
        }

        public void ResetInventory()
        {
            throw new System.NotImplementedException();
        }

        public void SwapSlots(int fromIndex, int toIndex, out ItemStack stack, out ItemStack toStack)
        {
            throw new System.NotImplementedException();
        }
    }
}