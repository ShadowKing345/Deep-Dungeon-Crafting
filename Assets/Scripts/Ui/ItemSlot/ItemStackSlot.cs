using System;
using Inventory;
using Items;
using Managers;
using TMPro;
using Ui.InventoryControllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.ItemSlot
{
    public class ItemStackSlot : MonoBehaviour, IItemStackSlot
        , IPointerEnterHandler, IPointerExitHandler
        , IPointerDownHandler, IPointerUpHandler
    {
        private WindowManager _windowManager;

        [SerializeField] protected Image icon;
        [SerializeField] protected TextMeshProUGUI amount;
        [SerializeField] protected Image backgroundImg;

        [SerializeField] protected int index;
        [SerializeField] protected ItemStack stack;
        [SerializeField] protected GameObject hoverPrefab;

        [SerializeField] protected SlotSprites sprites;
        
        public int GetIndex() => index;
        private IInventoryController _controller;
        public IInventoryController GetController() => _controller;
        
        private void Start()
        {
            _windowManager = WindowManager.instance;
            backgroundImg.sprite = sprites.normal; 
            UpdateUi();
        }

        public void Init(int index, ItemStack stack, IInventoryController controller)
        {
            this.index = index;
            _controller = controller;
            SetItemStack(stack);
        }
        public void SetItemStack(ItemStack stack) => this.stack = stack ?? ItemStack.Empty;
        public ItemStack GetItemStack() => stack;

        public virtual void UpdateUi()
        {
            if (stack.IsEmpty)
            {
                icon.sprite = null;
                icon.color = Color.clear;
                amount.text = string.Empty;
            }
            else
            {
                icon.sprite = stack.Item.icon;
                icon.color = Color.white;
                amount.text = stack.Amount > 1 ? stack.Amount.ToString() : string.Empty;
            }
        }

        public void ResetSlot() => SetItemStack(ItemStack.Empty);

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;

            IItemStackSlot slot = eventData.pointerDrag.GetComponent<IItemStackSlot>();
            if (slot == null || slot.GetItemStack().IsEmpty) return;

            if (slot.GetController() != _controller)
                _controller.CrossControllerExchange(slot, this);
            else
                _controller.ExchangeItemStacks(slot.GetIndex(), index);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (stack == null || stack.IsEmpty) return;
            
            _windowManager.BeginItemHover(hoverPrefab, stack);
        }

        public void OnDrag(PointerEventData eventData) { }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            _windowManager.EndItemHover();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            backgroundImg.sprite = sprites.mouseEnter;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            backgroundImg.sprite = sprites.normal;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            backgroundImg.sprite = sprites.active;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            backgroundImg.sprite = sprites.normal;
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