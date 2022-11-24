using Project.Runtime.Enums;
using Project.Runtime.Managers;
using Project.Runtime.Utils;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Ui.Menu
{
    public class LevelComplete : MonoBehaviour, IUiWindow
    {
        private UiManager _uiManager;
        private CanvasGroup cg;

        private void Awake()
        {
            _uiManager = UiManager.Instance;
            cg = GetComponent<CanvasGroup>();

            _uiManager.RegisterWindow(WindowReference.CompleteScreen, gameObject);

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _uiManager.UnregisterWindow(WindowReference.CompleteScreen, gameObject);
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

        public void OnContinue()
        {
            SceneManager.Instance.ChangeScene(SceneIndexes.Hub);
        }
    }
}