using System;
using Managers;
using UnityEngine;

namespace Ui
{
    public class CloseWindow : MonoBehaviour
    {
        [SerializeField] private UiManager.UiElementReference uiElement;
        public void OnButtonClicked() => UiManager.Instance.HideUiElement(uiElement);
    }
}