using Project.Runtime.Systems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Runtime.Ui.ToolTip
{
    public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // private static LTDescr _delay;

        [SerializeField] private string header;

        [Multiline] [SerializeField] private string content;

        public string Header
        {
            set => header = value;
        }

        public string Content
        {
            set => content = value;
        }

        private void OnDisable()
        {
            // if (_delay != null) LeanTween.cancel(_delay.uniqueId);

            ToolTipSystem.Instance.HideToolTip(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ToolTipSystem.Instance.ShowToolTip(content, header);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ToolTipSystem.Instance.HideToolTip(true);
        }
    }
}