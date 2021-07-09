using System;
using System.Collections.Generic;
using InGameHelp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Help
{
    public class HelpController : MonoBehaviour
    {
        [Header("Entry Collection")]
        [SerializeField] private TabCollection collection;
        [Space]
        [Header("Navigation")]
        [SerializeField] private GameObject navigationContent;
        [SerializeField] private GameObject tabPreFab;
        [Space]
        [Header("Page")]
        [SerializeField] private PageContent pageContent;
        [Space]
        [SerializeField] private List<HelpTab> tabs = new List<HelpTab>();
        
        private void Start()
        {
            if (collection == null || tabPreFab == null) return;
            tabs.Clear();
            SetUpTabs();
            SetUpSubTabs();
            
            if(tabPreFab != null) tabPreFab.SetActive(false);
        }

        private void SetUpTabs()
        {
            foreach (Tab tab in collection.tabs) CreateTab(tab);
        }

        private void CreateTab(Tab tab)
        {
            GameObject obj = Instantiate(tabPreFab, navigationContent.transform);
            obj.SetActive(true);
            if (!obj.TryGetComponent(out HelpTab ui)) return;
            
            ui.controller = this;
            ui.Tab = tab;
            
            tabs.Add(ui);

            foreach (Tab subTab in tab.subTabs) CreateTab(subTab);
        }

        private void SetUpSubTabs()
        {
            foreach (HelpTab tabUi in tabs)
            {
                foreach (Tab subTab in tabUi.tab.subTabs)
                {
                    HelpTab subHelpTab = tabs.Find(t => t.tab == subTab);
                    if(subHelpTab != null)
                        subHelpTab.transform.SetParent(tabUi.SubTabObj.transform);
                }
            }
        }
        
        public void AlterPage(Page page)
        {
            pageContent.pageImage.sprite = page.icon;
            pageContent.nameText.text = page.name;
            pageContent.descriptionText.text = page.description;
        }
    }
    
    [Serializable]
    public struct PageContent
    {
        public Image pageImage;
        public TextMeshProUGUI nameText;

        public TextMeshProUGUI descriptionText;

        public GameObject weaponClassAbilityContainer;
    }

}
