using System;
using System.Collections.Generic;
using Systems;
using TMPro;
using Ui.ToolTip;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui.ContextMenu
{
    public class ContextMenuEntry : Button
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private ToolTipTrigger trigger;

        private Action callback;

        public void SetUp(string content, Action callBack)
        {
                text.text = trigger.Content = content;
                callback = callBack;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    callback?.Invoke();
                    onClick?.Invoke();
                    ContextMenuSystem.Instance.HideContextMenu();
                    break;
                case PointerEventData.InputButton.Right:
                    break;
                case PointerEventData.InputButton.Middle:
                    break;
                default:
                    break;
            }
        }
    }
}