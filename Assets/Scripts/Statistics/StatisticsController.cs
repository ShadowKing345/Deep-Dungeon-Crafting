using System.Collections.Generic;
using Interfaces;
using Ui.Statistics;
using UnityEngine;
using Utils;

namespace Statistics
{
    public class StatisticsController : MonoBehaviour, IUiWindow
    {
        private StatisticsManager _statisticsManager;
        
        [SerializeField] private Transform content;
        [SerializeField] private GameObject entryPreFab;

        private void OnEnable()
        {
            _statisticsManager = StatisticsManager.Instance;
            GameObjectUtils.ClearChildren(content);
            SetUpStatEntries(_statisticsManager.Dictionary, content);
        }
        
        private void SetUpStatEntries(Dictionary<string, object> dictionary, Transform parent)
        {
            if (dictionary == null) return;
            
            foreach (KeyValuePair<string,object> kvPair in dictionary)
            {
                GameObject entryObj = Instantiate(entryPreFab, parent);
                if (entryObj.TryGetComponent(out StatisticsEntry entry)) entry.Init = kvPair;
            }
            
            Canvas.ForceUpdateCanvases();
        }
        
        public void Show() { }

        public void Hide() { }
    }
}