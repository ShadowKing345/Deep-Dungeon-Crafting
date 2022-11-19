using System;
using Entity.Player;
using Enums;
using Interfaces;
using Managers;
using UnityEngine;

namespace Ui.Menu
{
    public class HelpMenu : MonoBehaviour, IUiWindow
    {
        private UiManager _uiManager;
        
        private PlayerMovement playerMovement;
        private PlayerCombat playerCombat;

        private void Awake()
        {
            _uiManager = UiManager.Instance;
            _uiManager.RegisterWindow(WindowReference.Help, gameObject);
            gameObject.SetActive(false);
        }

        private void OnDestroy() => _uiManager.UnregisterWindow(WindowReference.Help, gameObject);

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