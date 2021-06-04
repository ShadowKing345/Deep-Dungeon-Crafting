using Inventory;
using Items;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.ItemSlot
{
    public class ItemStackSlot : MonoBehaviour, IItemStackSlot
    {
        private WindowManager _windowManager;
        
        [SerializeField] private int index;
        public int GetIndex() => index;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;
        [SerializeField] private ItemStack stack;
        [SerializeField] private GameObject hoverPrefab;
        private IInventoryController _controller;
        public IInventoryController GetController() => _controller;
        
        private void Start()
        {
            _windowManager = WindowManager.instance;
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

        public void UpdateUi()
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
    }
}