using Interfaces;
using Managers;
using UnityEngine;

namespace Ui.Menu
{
    public class PauseMenu : MonoBehaviour, IUiElement
    {
        private GameManager gameManager;
        private UiManager _uiManager;
        [SerializeField] private Settings settingsMenu;

        private void Start()
        {
            gameManager ??= GameManager.instance;
            _uiManager ??= UiManager.Instance;
        }

        public void Show()
        {
            Time.timeScale = 0;
        }

        public void Hide()
        {
            if(settingsMenu.gameObject.activeSelf) settingsMenu.Hide();
            Time.timeScale = 1;
        }

        public void Resume()
        {
            _uiManager.HideUiElement(UiManager.UiElementReference.PauseMenu);
        }

        public void OpenHelp()
        {
            Resume();
            _uiManager.ToggleUiElement(UiManager.UiElementReference.Help);
        }

        public void Settings()
        {
            settingsMenu.Show();
        }

        public void EndRun()
        {
            gameManager.EndRun();
        }

        public void QuitGame()
        {
            gameManager.QuitGame();
        }

        public void ExitGame()
        {
            gameManager.ExitGame();
        }
    }
}