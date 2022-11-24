using Project.Runtime.Systems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Runtime.Ui.Inventories.ItemSlot.Utils
{
    [RequireComponent(typeof(IItemStackSlot))]
    public class ItemStackSlotHover : Selectable
    {
        private IItemStackSlot slot;

        protected override void OnEnable()
        {
            base.OnEnable();
            slot ??= GetComponent<IItemStackSlot>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ToolTipSystem.Instance.HideToolTip(item: true);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!slot.ItemStack.IsEmpty) ToolTipSystem.Instance.ShowToolTIp(slot.ItemStack);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            ToolTipSystem.Instance.HideToolTip(item: true);
        }
    }
}