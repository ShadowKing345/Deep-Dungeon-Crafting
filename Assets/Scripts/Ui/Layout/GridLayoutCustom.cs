using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Layout
{
    public class GridLayoutCustom : LayoutGroup
    {
        public int columns;
        public int rows;
        public Vector2 cellSize;
        public ForceParentSize forceParentSize;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            var rect = rectTransform.rect;

            if (forceParentSize.width)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (cellSize.x + padding.horizontal) * columns);
            }
            else
            {
                try
                {
                    cellSize.x = rect.width / columns;
                }
                catch (DivideByZeroException)
                {
                    cellSize.x = rect.width;
                }
            }

            foreach (RectTransform item in rectChildren)
            {
                int index = rectChildren.IndexOf(item);
                int columnCount = columns != 0 ? index % columns : index;
                SetChildAlongAxis(item, 0, cellSize.x * columnCount);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            rows = columns != 0 ? rectChildren.Count / columns : rectChildren.Count;

            if (forceParentSize.height)
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y * rows);
            else
                cellSize.y = rectTransform.rect.height / rows;


            foreach (var item in rectChildren)
            {
                int index = rectChildren.IndexOf(item);
                int rowCount = columns != 0 ? index / columns : index;

                SetChildAlongAxis(item, 1, cellSize.y * rowCount);
            }
        }

        public override void SetLayoutHorizontal() { }
        public override void SetLayoutVertical() { }
    }

    [Serializable]
    public struct ForceParentSize
    {
        public bool width;
        public bool height;
    }
}
