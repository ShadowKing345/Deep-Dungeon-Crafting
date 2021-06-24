using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Journal
{
    public class TabUi : MonoBehaviour
    {
        [SerializeField] public Tab tab;
        [SerializeField] private JournalController controller;
        
        [SerializeField] private TextMeshProUGUI text;
        
        [SerializeField] private Button tabButton;
        [SerializeField] private Button expandButton;

        [SerializeField] private GameObject subTabPreFab;
        [SerializeField] private GameObject subTabObj;
        public GameObject SubTabObj => subTabObj;

        private bool isSubTabExpanded;

        public void SetTab(Tab tab, JournalController controller)
        {
            this.tab = tab;
            this.controller = controller;

            if (tabButton != null)
            {
                tabButton.onClick.RemoveAllListeners();
                tabButton.onClick.AddListener(OnButtonClick);
            }

            if (expandButton != null)
            {
                expandButton.onClick.RemoveAllListeners();
                expandButton.onClick.AddListener(OnExpandButtonClick);
            }

            UpdateUi();
        }

        private void UpdateUi()
        {
            text.text = tab.name;
        }

        private void OnButtonClick()
        {
            if (tab.page == null) return;

            controller.AlterPage(tab.page);
        }

        private void OnExpandButtonClick()
        {
            if (tab.subTabs.Length <= 0)
            {
                OnButtonClick();
            }
            else
            {
                isSubTabExpanded = !isSubTabExpanded;
             
                if (subTabObj.TryGetComponent(out ContentSizeFitter contentSizeFitter))
                {
                    contentSizeFitter.enabled = isSubTabExpanded;
                }

                if (subTabObj.TryGetComponent(out RectTransform rt) && !isSubTabExpanded)
                {
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                }
            }
        }
    }
}