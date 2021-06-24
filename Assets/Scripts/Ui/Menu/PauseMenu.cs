using Interfaces;
using Managers;
using UnityEngine;

namespace Ui.Menu
{
    public class PauseMenu : MonoBehaviour, IUiElement
    {
        private WindowManager windowManager;
        [SerializeField] private Settings settingsMenu;

        private void Start() => windowManager ??= WindowManager.instance;

        public void Show()
        {
            windowManager.HideUiElement(WindowManager.UiElementReference.Hud);
            Time.timeScale = 0;
        }

        public void Hide()
        {
            windowManager.ShowUiElement(WindowManager.UiElementReference.Hud);
            Time.timeScale = 1;
        }

        public void Resume()
        {
            WindowManager.instance.HideUiElement(WindowManager.UiElementReference.PauseMenu);
        }

        public void OpenJournal()
        {
            WindowManager.instance.HideUiElement(WindowManager.UiElementReference.PauseMenu);
            WindowManager.instance.ToggleUiElement(WindowManager.UiElementReference.Journal);
        }

        public void Settings()
        {
            settingsMenu.Show();
        }

        public void EndRun()
        {
            Debug.Log("End Run");
        }

        public void QuitGame()
        {
            Debug.Log("Quit Game");
        }

        public void ExitGame()
        {
            Debug.Log("Exit Game");
        }
    }
}