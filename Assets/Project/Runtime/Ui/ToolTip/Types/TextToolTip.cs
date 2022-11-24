using Project.Runtime.Utils.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.Ui.ToolTip.Types
{
    public class TextToolTip : FollowMouse
    {
        [SerializeField] private TextMeshProUGUI headerTMP;
        [SerializeField] private TextMeshProUGUI contentTMP;
        [SerializeField] private LayoutElement layoutElement;

        [SerializeField] private int characterWrapLimit;

        public void UpdateContent(string content, string header = "")
        {
            headerTMP.text = header;
            headerTMP.gameObject.SetActive(!string.IsNullOrEmpty(header));

            contentTMP.text = content;

            layoutElement.enabled = headerTMP.text.Length > characterWrapLimit ||
                                    contentTMP.text.Length > characterWrapLimit;
        }
    }
}