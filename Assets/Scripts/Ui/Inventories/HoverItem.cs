using Items;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Ui;

namespace Ui.Inventories
{
    public class HoverItem : FollowMouse
    {
        [SerializeField] private Image image;

        private void Awake() => image ??= GetComponent<Image>();
        
        public void Init(ItemStack stack)
        {
            if (stack == null || stack.IsEmpty) return;
            
            image.sprite = stack.Item.Icon;
        }
    }
}