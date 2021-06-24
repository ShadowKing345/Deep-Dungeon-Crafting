using UnityEngine;
using UnityEngine.UI;

namespace Ui.Layout
{
    [ExecuteInEditMode]
    public class PixelPerfectPadding : LayoutGroup
    {
        [SerializeField] private RectTransform[] targets;
        [SerializeField] private Image image;
        [SerializeField] private float wUpp;
        [SerializeField] private float hUpp;
        [SerializeField] private float resolution;
        [SerializeField] private CanvasScaler canvas;

        protected override void Start()
        {
            canvas ??= GetComponentInParent<CanvasScaler>();
        }

        public override void CalculateLayoutInputVertical()
        {
            if(image == null || image.sprite == null) return;
            
            Rect rect = rectTransform.rect;
            Rect imageRect = image.sprite.rect;

            if (image.type == Image.Type.Simple)
            {
                wUpp = rect.width / imageRect.width;
                hUpp = rect.height / imageRect.height;

                Vector2 xy = new Vector2(padding.left * wUpp, padding.top * hUpp * -1);
                Vector2 widthHeight = new Vector2(rect.width - xy.x - wUpp * padding.right,
                    rect.height + xy.y - hUpp * padding.bottom);

                foreach (RectTransform target in targets)
                {
                    target.anchorMin = target.anchorMax = Vector2.up;
                    target.anchoredPosition = xy + new Vector2(0, -widthHeight.y) + widthHeight * target.pivot;
                    target.sizeDelta = widthHeight;
                }
            }

            if (image.type == Image.Type.Sliced)
            {
                if(canvas == null) return;
                
                resolution = canvas.referencePixelsPerUnit / image.sprite.pixelsPerUnit;

                Vector2 xy = new Vector2(resolution * padding.left, padding.top * resolution * -1);
                Vector2 widthHeight = new Vector2(rect.width - xy.x - padding.right * resolution, rect.height + xy.y - padding.bottom * resolution);
                
                foreach (RectTransform target in targets)
                {
                    target.anchorMin = target.anchorMax = Vector2.up;
                    target.anchoredPosition = xy + new Vector2(0, -widthHeight.y) +  widthHeight * target.pivot;
                    target.sizeDelta = widthHeight;
                }
            }
        }
        
        public override void SetLayoutHorizontal() { }
        public override void SetLayoutVertical() { }
    }
}