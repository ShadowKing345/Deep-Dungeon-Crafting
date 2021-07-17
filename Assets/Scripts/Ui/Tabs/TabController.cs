using System;
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
        
        private readonly Dictionary<Tab, GameObject> tabPageDict = new Dictionary<Tab, GameObject>();
        private GameObject activePage;

        public Dictionary<Tab, GameObject> TabPageDict => tabPageDict;

        private void OnEnable()
        {
            tabPageDict.Clear();
            
            GameObjectUtils.ClearChildren(navigationContent.transform);
            GameObjectUtils.ClearChildren(bodyContent.transform);
        }

        public void AddPage(Tab tab, GameObject pageObj)
        {
            pageObj.SetActive(false);
            tabPageDict.Add(tab, pageObj);
        }

        public void RemovePage(Tab tab) => tabPageDict.Remove(tab);

        public void ChangePage(Tab tab)
        {
            if (!tabPageDict.TryGetValue(tab, out GameObject page)) return;
            if (page == activePage) return;
                
            foreach (KeyValuePair<Tab,GameObject> kvPair in tabPageDict) kvPair.Value.SetActive(kvPair.Key == tab);
            activePage = page;
            activePage.GetComponentInChildren<Selectable>().Select();
        }
    }
}