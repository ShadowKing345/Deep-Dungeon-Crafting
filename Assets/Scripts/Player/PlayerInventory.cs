using Ui.InventoryControllers;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public InventoryController inventoryController;
        public WeaponInventoryController weaponInventoryController;
        
        public Inventory.Inventory inventory;
        public Inventory.ArmorInventory armorInventory;
        public Inventory.WeaponInventory weaponInventory;
        
        private void Start()
        {
            inventoryController.Init(inventory);
            weaponInventoryController.Init(weaponInventory);
        }
    }
}