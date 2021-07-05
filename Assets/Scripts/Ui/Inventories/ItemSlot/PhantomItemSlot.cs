using Inventory;
using Items;
using TMPro;
using Ui.Inventories.InventoryControllers;
using Ui.ToolTip;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.Inventories.ItemSlot
{
    public class PhantomItemSlot : MonoBehaviour, IItemStackSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ItemStack stack;
        [Space]
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI text;

        public int Id { get; set; }
        public ItemStack ItemStack
        {
            get => stack;
            set
            {
                stack = value;
                UpdateUi();
            }
        }
        public IInventoryController Controller { get; set; }
        public int InventoryIndex { get; set; }
        public IInventory Inventory { get; set; }

        public void Init(int id, ItemStack stack, IInventoryController controller, int inventoryIndex, IInventory inventory)
        {
            Id = id;
            ItemStack = stack;
            Controller = controller;
            InventoryIndex = inventoryIndex;
            Inventory = inventory;
        }

        public void UpdateUi()
        {
                icon.sprite = stack.IsEmpty ? null : stack.Item.Icon;
                icon.color = stack.IsEmpty ? Color.clear : Color.white;
                text.text = stack.IsEmpty ? string.Empty : stack.Amount.ToString();
        }

        public void ResetSlot() => ItemStack = ItemStack.Empty;
        
        public bool CanFit(ItemStack _) => true;

        #region Unused

        public void OnDrag(PointerEventData eventData) { }

        public void OnBeginDrag(PointerEventData eventData) { }

        public void OnDrop(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData) { }

        #endregion

        public void OnPointerEnter(PointerEventData eventData) => ToolTipSystem.instance.ShowItemToolTip(stack);

        public void OnPointerExit(PointerEventData eventData) => ToolTipSystem.instance.HideItemToolTip();
    }
}