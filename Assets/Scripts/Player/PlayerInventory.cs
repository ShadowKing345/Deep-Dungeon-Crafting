using Items;
using Managers;
using Ui.InventoryControllers;
using UnityEngine;
using Utils;

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
            armorInventory.AddStackAtSlot(ItemStack.Empty, 4);
        }

        private void Update()
        {
            if (InputHandler.instance.GetKeyDown(InputHandler.KeyValue.OpenInventory))
                WindowManager.instance.ToggleCharacterMenu();
        }
    }
}