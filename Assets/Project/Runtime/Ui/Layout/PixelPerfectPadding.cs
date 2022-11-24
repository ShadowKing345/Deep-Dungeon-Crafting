using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.Ui.Layout
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
            if (image == null || image.sprite == null) return;

            var rect = rectTransform.rect;
            var imageRect = image.sprite.rect;
            Vector2 xy;
            Vector2 widthHeight;

            if (image.type == Image.Type.Simple)
            {
                wUpp = rect.width / imageRect.width;
                hUpp = rect.height / imageRect.height;

                xy = new Vector2(padding.left * wUpp, padding.top * hUpp * -1);
                widthHeight = new Vector2(rect.width - xy.x - wUpp * padding.right,
                    rect.height + xy.y - hUpp * padding.bottom);

                foreach (var target in targets)
                {
                    target.anchorMin = target.anchorMax = Vector2.up;
                    target.anchoredPosition = xy + new Vector2(0, -widthHeight.y) + widthHeight * target.pivot;
                    target.sizeDelta = widthHeight;
                }
            }

            if (image.type != Image.Type.Sliced) return;

            if (canvas == null) return;

            resolution = canvas.referencePixelsPerUnit / image.sprite.pixelsPerUnit;

            xy = new Vector2(resolution * padding.left, padding.top * resolution * -1);
            widthHeight = new Vector2(rect.width - xy.x - padding.right * resolution,
                rect.height + xy.y - padding.bottom * resolution);

            foreach (var target in targets)
            {
                target.anchorMin = target.anchorMax = Vector2.up;
                target.anchoredPosition = xy + new Vector2(0, -widthHeight.y) + widthHeight * target.pivot;
                target.sizeDelta = widthHeight;
            }
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }
    }
}