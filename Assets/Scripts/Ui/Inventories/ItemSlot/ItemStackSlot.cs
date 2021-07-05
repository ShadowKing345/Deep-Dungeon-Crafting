using System;
using Inventory;
using Items;
using Managers;
using TMPro;
using Ui.Inventories.InventoryControllers;
using Ui.ToolTip;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.Inventories.ItemSlot
{
    public class ItemStackSlot : MonoBehaviour, IItemStackSlot,
        IPointerEnterHandler, IPointerExitHandler
        , IPointerDownHandler, IPointerUpHandler
    {
        private WindowManager _windowManager;

        [SerializeField] private ItemStack stack;
        [Space]
        [SerializeField] protected Image icon;
        [SerializeField] protected TextMeshProUGUI amount;
        [SerializeField] protected Image backgroundImg;
        [Space]
        [SerializeField] protected SlotSprites sprites;
        
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


        private void Start()
        {
            _windowManager = WindowManager.instance;
            backgroundImg.sprite = sprites.normal; 
            UpdateUi();
        }

        public void Init(int id, ItemStack stack, IInventoryController controller, int inventoryIndex, IInventory inventory)
        {
            Id = id;
            Controller = controller;
            ItemStack = stack;
            InventoryIndex = inventoryIndex;
            Inventory = inventory;
        }
        
        public virtual void UpdateUi()
        {
            if (ItemStack.IsEmpty)
            {
                icon.sprite = null;
                icon.color = Color.clear;
                amount.text = string.Empty;
            }
            else
            {
                icon.sprite = ItemStack.Item.Icon;
                icon.color = Color.white;
                amount.text = ItemStack.Amount > 1 ? ItemStack.Amount.ToString() : string.Empty;
            }
        }

        public void ResetSlot() => ItemStack = ItemStack.Empty;

        public bool CanFit(ItemStack stack) => Inventory.CanFitInSlot(stack, InventoryIndex);

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;

            IItemStackSlot slot = eventData.pointerDrag.GetComponent<IItemStackSlot>();
            if (slot == null || slot.ItemStack.IsEmpty) return;

            Controller.ExchangeItemStacks(slot, this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (ItemStack == null || ItemStack.IsEmpty) return;
            
            _windowManager.BeginItemHover(ItemStack);
        }

        public void OnDrag(PointerEventData eventData) { }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            _windowManager.EndItemHover();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            backgroundImg.sprite = sprites.mouseEnter;
            if (!stack.IsEmpty) ToolTipSystem.instance.ShowItemToolTip(stack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            backgroundImg.sprite = sprites.normal;
            ToolTipSystem.instance.HideItemToolTip();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            backgroundImg.sprite = sprites.active;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            backgroundImg.sprite = sprites.normal;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Test");
        }
    }

    [Serializable]
    public struct SlotSprites
    {
        public Sprite normal;
        public Sprite mouseEnter;
        public Sprite active;
    }
}