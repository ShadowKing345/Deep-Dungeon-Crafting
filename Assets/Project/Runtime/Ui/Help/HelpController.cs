using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Ui.InGameHelp;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Help
{
    //todo: Dear god why the actual f did i not come back and completely redo this system
    public class HelpController : MonoBehaviour
    {
        [Header("Entry Collection")] [SerializeField]
        private TabCollection collection;

        [Space] [Header("Navigation")] [SerializeField]
        private GameObject navigationContent;

        [SerializeField] private GameObject tabPreFab;

        [Space] [Header("Page")] [SerializeField]
        private PageContent pageContent;

        [Space] [SerializeField] private List<HelpEntry> entries = new();

        private void OnEnable()
        {
            if (collection == null || tabPreFab == null) return;

            foreach (var entry in entries) Destroy(entry.gameObject);
            entries.Clear();

            if (tabPreFab == null) return;

            SetUpTabs();
            SetUpSubTabs();
            tabPreFab.SetActive(false);

            var first = entries.FirstOrDefault();
            if (!first) return;
            first.GetComponentInChildren<Selectable>().Select();
            AlterPage(first.tab.page);
        }

        private void SetUpTabs()
        {
            foreach (var tab in collection.tabs) CreateTab(tab);
        }

        private void CreateTab(Tab tab)
        {
            var obj = Instantiate(tabPreFab, navigationContent.transform);
            obj.SetActive(true);
            if (!obj.TryGetComponent(out HelpEntry ui)) return;

            ui.controller = this;
            ui.Tab = tab;

            entries.Add(ui);

            foreach (var subTab in tab.subTabs) CreateTab(subTab);
        }

        private void SetUpSubTabs()
        {
            foreach (var tabUi in entries)
            foreach (var subTab in tabUi.tab.subTabs)
            {
                var subHelpEntry = entries.Find(t => t.tab == subTab);
                if (subHelpEntry != null)
                    subHelpEntry.transform.SetParent(tabUi.SubTabObj.transform);
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