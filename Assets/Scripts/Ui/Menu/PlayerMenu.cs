using Entity.Player;
using Interfaces;
using Ui.Inventories.InventoryControllers;
using UnityEngine;

namespace Ui.Menu
{
    public class PlayerMenu : MonoBehaviour, IUiElement
    {
        [SerializeField] private PlayerMovement playerMovementController;
        [SerializeField] private PlayerCombat playerCombatController;
        [SerializeField] private PlayerInventoryController playerInventoryController;

        private void Start()
        {
            playerMovementController ??= FindObjectOfType<PlayerMovement>();
            playerCombatController ??= FindObjectOfType<PlayerCombat>();
            playerInventoryController ??= PlayerInventoryController.Instance;
        }

        public void Show()
        {
            playerMovementController.IsEnabled = false;
            playerCombatController.IsEnabled = false;
            
            playerInventoryController.UpdateSlots();
        }

        public void Hide()
        {
            playerMovementController.IsEnabled = true;
            playerCombatController.IsEnabled = true;
        }
    }
}