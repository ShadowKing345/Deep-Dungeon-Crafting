using Interfaces;
using Managers;
using UnityEngine;

namespace Ui.Menu
{
    public class PauseMenu : MonoBehaviour, IUiElement
    {
        private GameManager gameManager;
        private WindowManager windowManager;
        [SerializeField] private Settings settingsMenu;

        private void Start()
        {
            gameManager ??= GameManager.instance;
            windowManager ??= WindowManager.instance;
        }

        public void Show()
        {
            Time.timeScale = 0;
        }

        public void Hide()
        {
            settingsMenu.Hide();
            Time.timeScale = 1;
        }

        public void Resume()
        {
            WindowManager.instance.HideUiElement(WindowManager.UiElementReference.PauseMenu);
        }

        public void OpenHelp()
        {
            Resume();
            WindowManager.instance.ToggleUiElement(WindowManager.UiElementReference.Help);
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