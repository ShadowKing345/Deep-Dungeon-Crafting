using Entity.Player;
using Interfaces;
using UnityEngine;

namespace Ui.Menu
{
    public class CraftingMenu: MonoBehaviour, IUiElement
    {
        [SerializeField] private PlayerMovement playerMovementController;
        [SerializeField] private PlayerCombat playerCombatController;

        private void Start()
        {
            playerMovementController ??= FindObjectOfType<PlayerMovement>();
            playerCombatController ??= FindObjectOfType<PlayerCombat>();
        }

        public void Show()
        {
            playerMovementController.IsEnabled = false;
            playerCombatController.IsEnabled = false;
        }

        public void Hide()
        {
            playerMovementController.IsEnabled = true;
            playerCombatController.IsEnabled = true;
        }
    }
}