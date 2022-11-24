using Ui.Inventories.ItemSlot;
using UnityEngine;

namespace Ui.Crafting
{
    public class IngredientItemStack : ItemStackSlot
    {
        [SerializeField] private GameObject notFilledImage;
        public bool HasItemStack { get; set; }

        public override void UpdateUi()
        {
            base.UpdateUi();
            notFilledImage.SetActive(!HasItemStack);
        }
    }
}