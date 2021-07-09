using System;
using Entity.Player;
using Interfaces;
using Ui.Crafting;
using UnityEngine;

namespace Ui.Menu
{
    public class CraftingMenu: MonoBehaviour, IUiElement
    {
        [SerializeField] private PlayerMovement playerMovementController;
        [SerializeField] private PlayerCombat playerCombatController;

        private void OnEnable()
        {
            playerMovementController ??= FindObjectOfType<PlayerMovement>();
            playerCombatController ??= FindObjectOfType<PlayerCombat>();
        }
        
        public void Show()
        {
            playerMovementController.IsEnabled = false;
            playerCombatController.IsEnabled = false;
            
            if(TryGetComponent(out CraftingController cc)) cc.CreateRecipeEntries();
        }

        public void Hide()
        {
            playerMovementController.IsEnabled = true;
            playerCombatController.IsEnabled = true;
        }
    }
}