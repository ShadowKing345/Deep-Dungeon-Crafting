using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui.ToolTip
{
    public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private static LTDescr _delay;
        
        [SerializeField] private string header;
        [Multiline] 
        [SerializeField] private string content;

        public void OnPointerEnter(PointerEventData eventData) => _delay = LeanTween.delayedCall(0.3f, _ => ToolTipSystem.instance.ShowToolTip(content, header));

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.cancel(_delay.uniqueId);
            ToolTipSystem.instance.HideToolTip();
        }
    }
}