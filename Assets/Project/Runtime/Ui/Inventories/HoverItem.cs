using Items;
using UnityEngine;
using UnityEngine.UI;
using Utils.Ui;

namespace Ui.Inventories
{
    public class HoverItem : FollowMouse
    {
        [SerializeField] private Image image;
        [SerializeField] private ItemStack stack;

        public ItemStack Stack
        {
            get
                => stack;
            set
            {
                stack = value;
                if (stack == null || stack.IsEmpty) return;
                image.sprite = stack.Item.Icon;
            }
        }

        private void Awake()
        {
            image ??= GetComponent<Image>();
        }
    }
}