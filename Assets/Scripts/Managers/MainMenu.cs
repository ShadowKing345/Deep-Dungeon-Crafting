using Interfaces;
using Ui.Saves;
using UnityEngine;

namespace Managers
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject settingsObj;
        [SerializeField] private GameObject savesObj;

        private void OnEnable()
        {
            settingsObj.SetActive(false);
            savesObj.SetActive(false);
        }

        public void OnNewGameClick()
        {
            savesObj.SetActive(true);
            if (savesObj.TryGetComponent(out IUiWindow window)) window.Show();
            if(savesObj.TryGetComponent(out SaveManager savesController)) savesController.State = SaveManager.ControllerState.NewGame;
        }

        public void OnLoadGameClick()
        {
            savesObj.SetActive(true);
            if (savesObj.TryGetComponent(out IUiWindow window)) window.Show();
            if(savesObj.TryGetComponent(out SaveManager savesController)) savesController.State = SaveManager.ControllerState.LoadGame;
        }

        public void OnSettingsClick()
        {
            settingsObj.SetActive(true);
            if (settingsObj.TryGetComponent(out IUiWindow window)) window.Show();
        }

        public void OnExitGameClick() => GameManager.Instance.ExitGame();

        public void OnBackClick()
        {
            if (settingsObj.activeSelf && settingsObj.TryGetComponent(out IUiWindow settingWindow)) settingWindow.Show();
            settingsObj.SetActive(false);
            if (savesObj.activeSelf && savesObj.TryGetComponent(out IUiWindow savesWindow)) savesWindow.Show();
            savesObj.SetActive(false);
        }
    }
}