using InGameHelp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Help
{
    public class HelpTab : MonoBehaviour
    {
        //todo: review and refactor.
        [SerializeField] public Tab tab;
        [SerializeField] private HelpController controller;
        [Space]
        [SerializeField] private TextMeshProUGUI text;
        [Space]
        [SerializeField] private GameObject subTabPreFab;
        [SerializeField] private GameObject subTabObj;
        [Space]
        [SerializeField] private Image dropDownImage;
        [Space]
        [SerializeField] private Sprite arrowDown;
        [SerializeField] private Sprite arrowUp;
         
        public GameObject SubTabObj => subTabObj;

        public void SetTab(Tab tab, HelpController controller)
        {
            this.tab = tab;
            this.controller = controller;

            UpdateUi();
        }

        private void UpdateUi()
        {
            text.text = tab.name;
            dropDownImage.enabled = tab.subTabs.Length > 0;
        }

        public void OnButtonClick()
        {
            if (tab.page == null) return;

            controller.AlterPage(tab.page);
        }

        public void OnExpandToggleClick(bool value)
        {
            if (tab == null) return;
            
            if (tab.subTabs.Length <= 0)
                OnButtonClick();
            else
            {
                dropDownImage.sprite = value ? arrowDown : arrowUp;
                if (subTabObj.TryGetComponent(out ContentSizeFitter contentSizeFitter))
                    contentSizeFitter.enabled = value;
                if (subTabObj.TryGetComponent(out RectTransform rt) && !value)
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            }
        }
    }
}