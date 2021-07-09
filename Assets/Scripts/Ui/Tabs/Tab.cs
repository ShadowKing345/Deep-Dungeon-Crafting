using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.Tabs
{
    public class Tab : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int pageNumber;
        [SerializeField] private TabController controller;

        private void OnEnable() => controller ??= GetComponentInParent<TabController>();

        public void OnPointerClick(PointerEventData eventData) => controller.ChangePage(pageNumber);
    }
}