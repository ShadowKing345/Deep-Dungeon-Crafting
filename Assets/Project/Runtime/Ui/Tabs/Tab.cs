using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Runtime.Ui.Tabs
{
    public class Tab : Selectable, IPointerClickHandler
    {
        [SerializeField] private TabController controller;
        [SerializeField] private Sprite sprite;

        [Space] [SerializeField] private Image iconImage;

        public Sprite Sprite
        {
            get => sprite;
            set
            {
                sprite = value;
                iconImage.sprite = value;
            }
        }

        protected override void OnEnable()
        {
            controller ??= GetComponentInParent<TabController>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            controller.ChangePage(this);
        }
    }
}