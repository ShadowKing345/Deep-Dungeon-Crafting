using System;
using Items;
using Ui.ToolTip.Types;
using UnityEngine;

namespace Ui.ToolTip
{
    public class ToolTipSystem : MonoBehaviour
    {
        public static ToolTipSystem instance;

        [SerializeField] public ItemToolTip itemToolTip;
        [SerializeField] public TextToolTip textToolTip;
        
        

        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);
        }

        public void ShowItemToolTip(ItemStack stack)
        {
            itemToolTip.gameObject.SetActive(true);
            itemToolTip.ItemStack = stack;
        }

        public void HideItemToolTip() => itemToolTip.gameObject.SetActive(false);

        public void ShowToolTip(string content, string header = "")
        {
            textToolTip.gameObject.SetActive(true);
            textToolTip.UpdateContent(content, header);
            LeanTween.alphaCanvas(textToolTip.canvasGroup, 1, 0.1f);
        }

        public void HideToolTip()
        {
            textToolTip.canvasGroup.alpha = 0;
            textToolTip.gameObject.SetActive(false);
        }
    }
}