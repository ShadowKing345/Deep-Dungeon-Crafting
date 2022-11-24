using Project.Runtime.Ui.Inventories.ItemSlot;
using UnityEngine;

namespace Project.Runtime.Ui.Crafting
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