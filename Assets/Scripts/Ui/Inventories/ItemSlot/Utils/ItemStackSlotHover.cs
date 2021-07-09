using System;
using Ui.ToolTip;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.Inventories.ItemSlot.Utils
{
    [RequireComponent(typeof(IItemStackSlot))]
    public class ItemStackSlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private IItemStackSlot slot;

        private void OnEnable() => slot ??= GetComponent<IItemStackSlot>();

        private void OnDisable() => ToolTipSystem.Instance.HideToolTip(item:true);
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!slot.ItemStack.IsEmpty) ToolTipSystem.Instance.ShowToolTIp(slot.ItemStack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ToolTipSystem.Instance.HideToolTip(item: true);
        }
    }
}