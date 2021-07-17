using System.Text.RegularExpressions;
using InGameHelp;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Help
{
    public class HelpEntry : MonoBehaviour
    {
        [SerializeField] public Tab tab;
        public HelpController controller;
        [Space]
        [SerializeField] private TextMeshProUGUI text;
        [Space]
        [Header("Sub Tab")]
        [SerializeField] private GameObject subTabObj;
        [Space]
        [Header("Expansion Button")]
        [SerializeField] private Selectable dropDownPointerChanger;
        [SerializeField] private Image dropDownImage;
        [SerializeField] private Sprite arrowDown;
        [SerializeField] private Sprite arrowUp;
        public GameObject SubTabObj => subTabObj;
        
        public Tab Tab
        {
            get => tab;
            set
            {
                tab = value;
                UpdateUi();
            }
        }

        private void UpdateUi()
        {
            text.text = tab.name;
            dropDownImage.enabled = dropDownPointerChanger.enabled = tab.subTabs.Length > 0;
        }

        public void OnButtonClick()
        {
            if (tab.page == null || controller == null) return;
            controller.AlterPage(tab.page);
        }

        public void OnExpandToggleClick(bool value)
        {
            if (tab == null || tab.subTabs.Length <= 0)
            {
                OnButtonClick();
                return;
            }
            
            dropDownImage.sprite = value ? arrowDown : arrowUp;
            if (subTabObj.TryGetComponent(out ContentSizeFitter contentSizeFitter))
                contentSizeFitter.enabled = value;
            if (subTabObj.TryGetComponent(out RectTransform rt) && !value)
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        }
    }
}