using Items;
using Managers;
using Ui.InventoryControllers;
using UnityEngine;
using Utils;

namespace Entity.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private InventoryController inventoryController;
        [SerializeField] private WeaponInventoryController weaponInventoryController;
        
        public Inventory.Inventory inventory;
        public Inventory.ArmorInventory armorInventory;
        public Inventory.WeaponInventory weaponInventory;
        
        private void Start()
        {
            inventoryController.Init(inventory);
            weaponInventoryController.Init(weaponInventory);
            armorInventory.AddStackAtSlot(ItemStack.Empty, 4);
        }
    }
}