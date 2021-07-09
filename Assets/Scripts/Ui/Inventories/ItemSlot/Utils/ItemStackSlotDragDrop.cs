using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.Inventories.ItemSlot.Utils
{
    [RequireComponent(typeof(IItemStackSlot))]
    public class ItemStackSlotDragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IDropHandler, IEndDragHandler
    {
        private UiManager _uiManager;

        private IItemStackSlot slot;
        
        private void OnEnable()
        {
            _uiManager ??= UiManager.Instance;
            slot ??= GetComponent<IItemStackSlot>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null) return;

            if(!eventData.pointerDrag.TryGetComponent(out IItemStackSlot fromSlot)) return;
            if (fromSlot == null || fromSlot.ItemStack.IsEmpty) return;

            slot.Controller.ExchangeItemStacks(fromSlot, slot);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (slot.ItemStack.IsEmpty) return;
            
            _uiManager.BeginItemHover(slot.ItemStack);
        }

        public void OnDrag(PointerEventData eventData) { }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            _uiManager.EndItemHover();
        }

    }
}