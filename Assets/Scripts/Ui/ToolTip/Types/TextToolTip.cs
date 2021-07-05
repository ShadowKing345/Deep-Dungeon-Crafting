using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Ui;

namespace Ui.ToolTip.Types
{
    public class TextToolTip : FollowMouse
    {
        public CanvasGroup canvasGroup;
        
        [SerializeField] private TextMeshProUGUI headerTMP;
        [SerializeField] private TextMeshProUGUI contentTMP;
        [SerializeField] private LayoutElement layoutElement;

        [SerializeField] private int characterWrapLimit;

        public void UpdateContent(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header)) headerTMP.gameObject.SetActive(false);
            else headerTMP.text = header;
            contentTMP.text = content;
            
            layoutElement.enabled = headerTMP.text.Length > characterWrapLimit || contentTMP.text.Length > characterWrapLimit;
        }
    }
}