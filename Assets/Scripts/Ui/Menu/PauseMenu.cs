using Entity.Player;
using Enums;
using Interfaces;
using Managers;
using Settings;
using Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Menu
{
    public class PauseMenu : MonoBehaviour, IUiWindow
    {
        private PlayerMovementManager playerMovementManager;
        private PlayerCombat playerCombat;

        private GameManager _gameManager;
        private UiManager _uiManager;
        [SerializeField] private SettingsController settingsMenu;
        [SerializeField] private StatisticsController statisticsMenu;

        private void Awake()
        {
            _gameManager ??= GameManager.Instance;
            _uiManager ??= UiManager.Instance;

            _uiManager.RegisterWindow(WindowReference.PauseMenu, gameObject);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            playerMovementManager ??= FindObjectOfType<PlayerMovementManager>();
            playerCombat ??= FindObjectOfType<PlayerCombat>();
        }

        private void OnDestroy()
        {
            _uiManager.UnregisterWindow(WindowReference.PauseMenu, gameObject);
        }

        public void Show()
        {
            if (playerCombat != null && playerMovementManager != null) playerCombat.enabled = playerMovementManager.enabled = false;
            Time.timeScale = 0;
            GetComponentInChildren<Selectable>().Select();
        }

        public void Hide()
        {
            if (playerCombat != null && playerMovementManager != null) playerCombat.enabled = playerMovementManager.enabled = true;
            if (settingsMenu.gameObject.activeSelf) settingsMenu.Hide();
            if (statisticsMenu.gameObject.activeSelf) statisticsMenu.Hide();
            Time.timeScale = 1;
        }

        public void Resume() => _uiManager.HideUiElement(WindowReference.PauseMenu);

        public void OpenHelp()
        {
            Resume();
            _uiManager.ToggleUiElement(WindowReference.Help);
        }

        public void Settings()
        {
            if (settingsMenu.gameObject.activeSelf)
                settingsMenu.Hide();
            else
                settingsMenu.Show();
        }

        public void Statistics()
        {
            if (statisticsMenu.gameObject.activeSelf)
                statisticsMenu.Hide();
            else
                statisticsMenu.Show();
        }

        public void EndRun()
        {
            _uiManager.HideUiElement(WindowReference.PauseMenu);
            _gameManager.EndRun();
        }

        public void QuitGame()
        {
            _uiManager.HideUiElement(WindowReference.PauseMenu);
            _gameManager.QuitGame();
        }

        public void ExitGame()
        {
            _uiManager.HideUiElement(WindowReference.PauseMenu);
            _gameManager.ExitGame();
        }
    }
}