using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ui.Tabs
{
    public class TabController : MonoBehaviour
    {
        [SerializeField] private GameObject navigationContent;
        [SerializeField] private GameObject bodyContent;

        private GameObject activePage;

        public Dictionary<Tab, GameObject> TabPageDict { get; } = new();

        private void OnEnable()
        {
            TabPageDict.Clear();

            GameObjectUtils.ClearChildren(navigationContent.transform);
            GameObjectUtils.ClearChildren(bodyContent.transform);
        }

        public void AddPage(Tab tab, GameObject pageObj)
        {
            pageObj.SetActive(false);
            TabPageDict.Add(tab, pageObj);
        }

        public void RemovePage(Tab tab)
        {
            TabPageDict.Remove(tab);
        }

        public void ChangePage(Tab tab)
        {
            if (!TabPageDict.TryGetValue(tab, out var page)) return;
            if (page == activePage) return;

            foreach (var kvPair in TabPageDict) kvPair.Value.SetActive(kvPair.Key == tab);
            activePage = page;
            activePage.GetComponentInChildren<Selectable>().Select();
        }
    }
}