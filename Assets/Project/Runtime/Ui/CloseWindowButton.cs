using Project.Runtime.Enums;
using Project.Runtime.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.Ui
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