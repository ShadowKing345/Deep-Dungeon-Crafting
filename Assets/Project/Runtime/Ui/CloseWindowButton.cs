using Enums;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class CloseWindowButton : Button
    {
        [SerializeField] private WindowReference uiElement;

        protected override void OnEnable()
        {
            base.OnEnable();
            onClick.AddListener(() => UiManager.Instance.HideUiElement(uiElement));
        }
    }
}