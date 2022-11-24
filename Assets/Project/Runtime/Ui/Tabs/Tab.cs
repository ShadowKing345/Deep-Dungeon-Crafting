using Ui.ToolTip;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.Tabs
{
    public class Tab : Selectable, IPointerClickHandler
    {
        [SerializeField] private TabController controller;
        [SerializeField] private Sprite sprite;

        [Space] [SerializeField] private Image iconImage;

        [SerializeField] private ToolTipTrigger toolTipTrigger;

        public Sprite Sprite
        {
            get => sprite;
            set
            {
                sprite = value;
                iconImage.sprite = value;
            }
        }

        public string ToolTipText
        {
            set => toolTipTrigger.Content = value;
        }

        protected override void OnEnable()
        {
            controller ??= GetComponentInParent<TabController>();
            toolTipTrigger ??= GetComponent<ToolTipTrigger>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            controller.ChangePage(this);
        }
    }
}