using Project.Runtime.Entity.Player;
using Project.Runtime.Enums;
using Project.Runtime.Managers;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.Ui.Menu
{
    public class PauseMenu : MonoBehaviour, IUiWindow
    {
        [SerializeField] private SettingsController settingsMenu;

        private GameManager _gameManager;
        private UiManager _uiManager;
        private PlayerCombat playerCombat;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            _gameManager ??= GameManager.Instance;
            _uiManager ??= UiManager.Instance;

            _uiManager.RegisterWindow(WindowReference.PauseMenu, gameObject);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            playerMovement ??= FindObjectOfType<PlayerMovement>();
            playerCombat ??= FindObjectOfType<PlayerCombat>();
        }

        private void OnDestroy()
        {
            _uiManager.UnregisterWindow(WindowReference.PauseMenu, gameObject);
        }

        public void Show()
        {
            if (playerCombat != null && playerMovement != null) playerCombat.enabled = playerMovement.enabled = false;
            Time.timeScale = 0;
            GetComponentInChildren<Selectable>().Select();
        }

        public void Hide()
        {
            if (playerCombat != null && playerMovement != null) playerCombat.enabled = playerMovement.enabled = true;
            if (settingsMenu.gameObject.activeSelf) settingsMenu.Hide();
            Time.timeScale = 1;
        }

        public void Resume()
        {
            _uiManager.HideUiElement(WindowReference.PauseMenu);
        }

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