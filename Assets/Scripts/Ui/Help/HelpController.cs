using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Player;
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
        [SerializeField] private List<HelpEntry> entries = new List<HelpEntry>();
        
        private void OnEnable()
        {
            if (collection == null || tabPreFab == null) return;

            foreach (HelpEntry entry in entries ) Destroy(entry.gameObject);
            entries.Clear();

            if (tabPreFab == null) return;
            
            SetUpTabs();
            SetUpSubTabs();
            tabPreFab.SetActive(false);

            entries.FirstOrDefault()?.GetComponentInChildren<Selectable>().Select();
        }

        private void SetUpTabs()
        {
            foreach (Tab tab in collection.tabs) CreateTab(tab);
        }

        private void CreateTab(Tab tab)
        {
            GameObject obj = Instantiate(tabPreFab, navigationContent.transform);
            obj.SetActive(true);
            if (!obj.TryGetComponent(out HelpEntry ui)) return;
            
            ui.controller = this;
            ui.Tab = tab;
            
            entries.Add(ui);

            foreach (Tab subTab in tab.subTabs) CreateTab(subTab);
        }

        private void SetUpSubTabs()
        {
            foreach (HelpEntry tabUi in entries)
            {
                foreach (Tab subTab in tabUi.tab.subTabs)
                {
                    HelpEntry subHelpEntry = entries.Find(t => t.tab == subTab);
                    if(subHelpEntry != null)
                        subHelpEntry.transform.SetParent(tabUi.SubTabObj.transform);
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
