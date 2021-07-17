using Entity.Player;
using Interfaces;
using Managers;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Menu
{
    public class PauseMenu : MonoBehaviour, IUiWindow
    {
        private PlayerMovement playerMovement;
        private PlayerCombat playerCombat;
        
        private GameManager gameManager;
        private UiManager _uiManager;
        [SerializeField] private SettingsController settingsMenu;

        private void OnEnable()
        {
            gameManager ??= GameManager.Instance;
            _uiManager ??= UiManager.Instance;
            playerMovement ??= FindObjectOfType<PlayerMovement>();
            playerCombat ??= FindObjectOfType<PlayerCombat>();
        }

        public void Show()
        {
            if(playerCombat != null && playerMovement != null) playerCombat.enabled = playerMovement.enabled = false;
            Time.timeScale = 0;
            GetComponentInChildren<Selectable>().Select();
        }

        public void Hide()
        {
            if(playerCombat != null && playerMovement != null) playerCombat.enabled = playerMovement.enabled = true;
            if(settingsMenu.gameObject.activeSelf) settingsMenu.Hide();
            Time.timeScale = 1;
        }

        public void Resume() => _uiManager.HideUiElement(UiManager.UiElementReference.PauseMenu);
        public void OpenHelp()
        {
            Resume();
            _uiManager.ToggleUiElement(UiManager.UiElementReference.Help);
        }
        public void Settings()
        {
            if (settingsMenu.gameObject.activeSelf)
                settingsMenu.Hide();
            else
                settingsMenu.Show();
        }

        public void EndRun() => gameManager.EndRun();
        public void QuitGame() => gameManager.QuitGame();
        public void ExitGame() => gameManager.ExitGame();
    }
}