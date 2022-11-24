using Project.Runtime.Ui.Menu;
using Project.Runtime.Ui.Saves;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Managers
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private SettingsController settingController;
        [SerializeField] private SaveController saveController;

        private void OnEnable()
        {
            settingController.gameObject.SetActive(false);
            saveController.gameObject.SetActive(false);
        }

        public void OnNewGameClick()
        {
            saveController.gameObject.SetActive(true);
            if (saveController.TryGetComponent(out IUiWindow window)) window.Show();
            saveController.New();
        }

        public void OnLoadGameClick()
        {
            saveController.gameObject.SetActive(true);
            if (saveController.TryGetComponent(out IUiWindow window)) window.Show();
            saveController.Load();
        }

        public void OnSettingsClick()
        {
            settingController.gameObject.SetActive(true);
            if (settingController.TryGetComponent(out IUiWindow window)) window.Show();
        }

        public void OnExitGameClick()
        {
            GameManager.Instance.ExitGame();
        }

        public void OnBackClick()
        {
            if (settingController.gameObject.activeSelf &&
                settingController.TryGetComponent(out IUiWindow settingWindow)) settingWindow.Hide();
            settingController.gameObject.SetActive(false);
            if (saveController.gameObject.activeSelf && saveController.TryGetComponent(out IUiWindow savesWindow))
                savesWindow.Hide();
            saveController.gameObject.SetActive(false);
        }
    }
}