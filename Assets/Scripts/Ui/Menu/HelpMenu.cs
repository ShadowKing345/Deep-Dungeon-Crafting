using System;
using Entity.Player;
using Interfaces;
using UnityEngine;

namespace Ui.Menu
{
    public class HelpMenu : MonoBehaviour, IUiWindow
    {
        private PlayerMovement playerMovement;
        private PlayerCombat playerCombat;

        private void OnEnable()
        {
            playerCombat ??= FindObjectOfType<PlayerCombat>();
            playerMovement ??= FindObjectOfType<PlayerMovement>();
        }

        public void Show()
        {
            playerCombat.enabled = playerMovement.enabled = false;
            Time.timeScale = 0;
        }

        public void Hide()
        {
            playerCombat.enabled = playerMovement.enabled = true;
            Time.timeScale = 1;
        }
    }
}