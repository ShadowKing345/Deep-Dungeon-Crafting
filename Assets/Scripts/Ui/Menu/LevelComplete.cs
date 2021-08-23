using System;
using Enums;
using Interfaces;
using Managers;
using UnityEngine;
using Utils;

namespace Ui.Menu
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

        private void OnDestroy() => _uiManager.UnregisterWindow(WindowReference.CompleteScreen, gameObject);

        public void OnContinue() => SceneManager.Instance.ChangeScene(SceneIndexes.Hub);

        public void Show()
        {
            GameManager.PlayerMovement = false;
            LeanTween.alphaCanvas(cg, 1, 0.5f).setOnComplete(_ => cg.interactable = cg.blocksRaycasts = true);
        }

        public void Hide()
        {
            GameManager.PlayerMovement = true;
            cg.interactable = cg.blocksRaycasts = false;
            LeanTween.alphaCanvas(cg, 1, 0.5f);
        }
    }
}