using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Journal
{
    public class JournalController : MonoBehaviour
    {
        [SerializeField] private TabCollection collection;

        [SerializeField] private GameObject tabPreFab;
        
        [SerializeField] private GameObject navigationContent;
        [SerializeField] private PageContent pageContent;

        [SerializeField] private List<TabUi> tabs = new List<TabUi>();
        
        private void Start()
        {
            if (collection == null || tabPreFab == null) return;
            tabs.Clear();
            SetUpTabs();
            SetUpSubTabs();
        }

        private void SetUpTabs()
        {
            foreach (Tab tab in collection.tabs)
            {
                CreateTab(tab);
            }
        }

        private void CreateTab(Tab tab)
        {
            GameObject obj = Instantiate(tabPreFab, navigationContent.transform);
            if (!obj.TryGetComponent(out TabUi ui)) return;
            
            ui.SetTab(tab, this);
            tabs.Add(ui);

            foreach (Tab subTab in tab.subTabs) CreateTab(subTab);
        }

        private void SetUpSubTabs()
        {
            foreach (TabUi tabUi in tabs)
            {
                foreach (Tab subTab in tabUi.tab.subTabs)
                {
                    TabUi subTabUi = tabs.Find(t => t.tab == subTab);
                    if(subTabUi != null)
                        subTabUi.transform.SetParent(tabUi.SubTabObj.transform);
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
