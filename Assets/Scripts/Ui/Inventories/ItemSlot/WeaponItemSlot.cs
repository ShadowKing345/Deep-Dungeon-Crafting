using UnityEngine;

namespace Ui.ItemSlot
{
    public class WeaponItemSlot : ItemStackSlot
    {
        [SerializeField] private Sprite defaultIcon;

        public override void UpdateUi() => icon.sprite = stack.IsEmpty ? defaultIcon : stack.Item.icon;
    }
}