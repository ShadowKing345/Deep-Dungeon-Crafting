using Entity.Player;
using Enums;
using Managers;
using UnityEngine;
using Utils;
using Utils.Interfaces;

namespace Ui.Menu
{
    public class DeathScreen : MonoBehaviour, IUiWindow
    {
        private UiManager _uiManager;

        private CanvasGroup cg;

        private void Awake()
        {
            _uiManager = UiManager.Instance;
            cg = GetComponent<CanvasGroup>();

            _uiManager.RegisterWindow(WindowReference.DeathScreen, gameObject);

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _uiManager.UnregisterWindow(WindowReference.DeathScreen, gameObject);
        }

        public void Show()
        {
            GameManager.PlayerMovement = false;
            // LeanTween.alphaCanvas(cg, 1, 0.5f).setOnComplete(_ => cg.interactable = cg.blocksRaycasts = true);
        }

        public void Hide()
        {
            GameManager.PlayerMovement = true;
            cg.interactable = cg.blocksRaycasts = false;
            // LeanTween.alphaCanvas(cg, 1, 0.5f);
        }

        public void OnEndRun()
        {
            FindObjectOfType<PlayerInventory>().ResetEveryInventory();
            SceneManager.Instance.ChangeScene(SceneIndexes.Hub);
        }

        public void OnQuit()
        {
            FindObjectOfType<PlayerInventory>().ResetEveryInventory();
            SceneManager.Instance.ChangeScene(SceneIndexes.MainMenu);
        }
    }
}