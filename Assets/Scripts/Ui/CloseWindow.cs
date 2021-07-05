using Managers;
using UnityEngine;

namespace Ui
{
    public class CloseWindow : MonoBehaviour
    {
        [SerializeField] private WindowManager.UiElementReference uiElement;
        public void OnButtonClicked() => WindowManager.instance.HideUiElement(uiElement);
    }
}