using UnityEngine;
using UnityEngine.UI;

namespace Ui.HudElements
{
    public class HealthBar : ProgressBar
    {
        [SerializeField] private Image fillImage;
        private bool IsFillImageNull => fillImage == null;
        
        [SerializeField] private Sprite fullSprite;
        [SerializeField] private Sprite midSprite;
        [SerializeField] private Sprite lowSprite;

        [SerializeField] private float midThreshold = 0.83f;
        [SerializeField] private float lowThreshold = 0.26f;
        
        public new void Update()
        {
            base.Update();
            if (IsFillImageNull) return;

            float percentage = Current / MAX;
            
            if (percentage <= lowThreshold)
                fillImage.sprite = lowSprite;
            else if (percentage <= midThreshold)
                fillImage.sprite = midSprite;
            else
                fillImage.sprite = fullSprite;
        }
    }
}