using UnityEngine;

namespace Ui.Inventories.ItemSlot
{
    public class WeaponItemSlot : ItemStackSlot
    {
        [SerializeField] private Sprite defaultIcon;

        public override void UpdateUi()
        {
            base.UpdateUi();
            icon.sprite = ItemStack.IsEmpty ? defaultIcon : ItemStack.Item.Icon;
        }
    }
}