using System.IO;
using UnityEngine;
using Inventory;
using Items;
using Managers;
using Utils;

namespace Entity.Player
{
    public class PlayerInventory : MonoBehaviour, IEntityInventoryController
    {
        [SerializeField] private ItemInventory itemInventory;
        [SerializeField] private WeaponInventory weaponInventory;
        [SerializeField] private ArmorInventory armorInventory;

        public ItemInventory ItemInventory => itemInventory;
        public WeaponInventory WeaponInventory => weaponInventory;
        public ArmorInventory ArmorInventory => armorInventory;

        public IInventory IndexMapping(int index, out int newIndex)
        {
            if (index <= itemInventory.Size)
            {
                newIndex = 0;
                return itemInventory;
            }
            
            if (index <= itemInventory.Size + armorInventory.Size)
            {
                newIndex = index - itemInventory.Size;
                return armorInventory;
            }

            newIndex = index - (armorInventory.Size + itemInventory.Size);
            return weaponInventory;
        }

        public IInventory InventoryMapping(int mappingIndex) =>
            mappingIndex switch
            {
                0 => itemInventory,
                1 => weaponInventory,
                2 => armorInventory,
                _ => null
            };

        public ItemStack AddStackAtSlot(ItemStack stack, int index) =>
            IndexMapping(index, out var newIndex).AddStackAtSlot(stack, newIndex);

        public ItemStack GetStackAtSlot(int index) => IndexMapping(index, out var newIndex).GetStackAtSlot(newIndex);

        public ItemStack RemoveStackAtSlot(int index) =>
            IndexMapping(index, out var newIndex).RemoveStackAtSlot(newIndex);

        public ItemStack[] AddItemStacks(ItemStack[] stacks, bool combine = true, int mappingIndex = 0) =>
            InventoryMapping(mappingIndex).AddItemStacks(stacks);

        public ItemStack[] GetItemStacks(int mappingIndex = 0) => InventoryMapping(mappingIndex).GetItemStacks();

        public ItemStack[] RemoveItemStacks(ItemStack[] stacks, int mappingIndex) =>
            InventoryMapping(mappingIndex).RemoveItemStacks(stacks);

        public ItemStack[] GetAndClearItemStacks(int mappingIndex) => InventoryMapping(mappingIndex).GetAndClearItemStacks();

        public bool Contains(ItemStack stack,int mappingIndex = 0) => InventoryMapping(mappingIndex).Contains(stack);

        public bool ContainsExact(ItemStack stack, int mappingIndex = 0) => InventoryMapping(mappingIndex).ContainsExact(stack);

        public bool CanFitInSlot(ItemStack stack, int index) => IndexMapping(index, out var newIndex).CanFitInSlot(stack, index);

        public bool CanFit(ItemStack stack, int mappingIndex = 0) => InventoryMapping(mappingIndex).CanFit(stack);

        public void ResetInventory(int mappingIndex = 0) => InventoryMapping(mappingIndex).ResetInventory();

        public void ResetEveryInventory()
        {
            armorInventory.ResetInventory();
            itemInventory.ResetInventory();
            weaponInventory.ResetInventory();
        }

        private void Awake()
        {
            Save save = SaveManager.Instance.GetCurrentSave;
            if (save == null) return;

            if (ItemManager.TryJsonToItemStacks(save.PlayerItemInventory, out ItemStack[] itemStacks))
            {
                itemInventory.ResetInventory();
                for (int i = 0; i < itemStacks.Length; i++) itemInventory.AddStackAtSlot(itemStacks[i], i);
            }
            
            if (ItemManager.TryJsonToItemStacks(save.PlayerWeaponInventory, out ItemStack[] weaponStacks))
            {
                weaponInventory.ResetInventory();
                for (int i = 0; i < weaponStacks.Length; i++) weaponInventory.AddStackAtSlot(weaponStacks[i], i);
            }

            if (ItemManager.TryJsonToItemStacks(save.PlayerArmorInventory, out ItemStack[] armorStacks))
            {
                armorInventory.ResetInventory();
                for (int i = 0; i < armorStacks.Length; i++) armorInventory.AddStackAtSlot(armorStacks[i], i);
            }
        }

        public bool SaveInventory { get; set; } = true;
        
        private void OnDestroy()
        {
            if (!SaveInventory) return;
            Save save = SaveManager.Instance.GetCurrentSave;
            if (save == null) return;

            if (ItemManager.TryItemStacksToJson(itemInventory.GetItemStacks(), out string itemJson))
                save.PlayerItemInventory = itemJson;
            if (ItemManager.TryItemStacksToJson(weaponInventory.GetItemStacks(), out string weaponJson))
                save.PlayerWeaponInventory = weaponJson;
            if (ItemManager.TryItemStacksToJson(armorInventory.GetItemStacks(), out string armorJson))
                save.PlayerArmorInventory = armorJson;
        }
    }
}