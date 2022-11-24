using System.Collections.Generic;
using Ui.Statistics;
using UnityEngine;
using Utils;
using Utils.Interfaces;

namespace Statistics
{
    public class StatisticsController : MonoBehaviour, IUiWindow
    {
        [SerializeField] private Transform content;
        [SerializeField] private GameObject entryPreFab;
        [SerializeField] private CanvasGroup canvasGroup;
        private StatisticsManager statisticsManager;

        private void OnEnable()
        {
            statisticsManager = StatisticsManager.Instance;
            GameObjectUtils.ClearChildren(content);
            SetUpStatEntries(new SortedDictionary<string, object>(statisticsManager.Dictionary), content);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            // _transition = LeanTween.alphaCanvas(canvasGroup, 1, 0.1f).setIgnoreTimeScale(true);
        }

        public void Hide()
        {
            // if(_transition != null) LeanTween.cancel(_transition.uniqueId);
            // LeanTween.alphaCanvas(canvasGroup, 0, 0.1f).setOnComplete(_ => gameObject.SetActive(false)).setIgnoreTimeScale(true);
        }

        private void SetUpStatEntries(IDictionary<string, object> dictionary, Transform parent)
        {
            if (dictionary == null) return;

            foreach (var kvPair in dictionary)
            {
                var entryObj = Instantiate(entryPreFab, parent);
                if (entryObj.TryGetComponent(out StatisticsEntry entry)) entry.Init = kvPair;
            }

            Canvas.ForceUpdateCanvases();
        }
    }
}