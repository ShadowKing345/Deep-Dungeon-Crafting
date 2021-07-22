using System;
using Systems;
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
            if (_delay != null) LeanTween.cancel(_delay.uniqueId);

            ToolTipSystem.Instance.HideToolTip(true);
        }

        public void OnPointerEnter(PointerEventData eventData) => _delay = LeanTween.delayedCall(0.3f, _ => ToolTipSystem.Instance.ShowToolTip(content, header)).setIgnoreTimeScale(true);

        public void OnPointerExit(PointerEventData eventData)
        {
            if(_delay != null) LeanTween.cancel(_delay.uniqueId);
            ToolTipSystem.Instance.HideToolTip(true);
        }
    }
}