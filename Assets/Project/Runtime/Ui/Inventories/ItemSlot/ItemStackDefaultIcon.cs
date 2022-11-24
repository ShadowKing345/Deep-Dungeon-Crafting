using UnityEngine;

namespace Project.Runtime.Ui.Inventories.ItemSlot
{
    public class ItemStackDefaultIcon : ItemStackSlot
    {
        [SerializeField] private Sprite defaultImage;

        public override void UpdateUi()
        {
            base.UpdateUi();

            if (!ItemStack.IsEmpty) return;

            icon.sprite = defaultImage;
            icon.color = Color.white;
        }
    }
}