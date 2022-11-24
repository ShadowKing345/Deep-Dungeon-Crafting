using System;
using Project.Runtime.Systems;
using Project.Runtime.Ui.ToolTip;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Runtime.Ui.ContextMenu
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
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}