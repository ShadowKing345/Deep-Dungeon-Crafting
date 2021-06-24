using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Layout
{
    public class GridLayoutCustom : LayoutGroup
    {
        public int columns;
        public int rows;
        public Vector2 cellSize;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            if (rectChildren.Count <= 0) return;

            cellSize.y = cellSize.x = rectTransform.rect.width / columns - padding.horizontal;
            float actualCellSize = cellSize.x + padding.horizontal;
            
            for(int i = 0; i < rectChildren.Count; i ++)
            {
                int colPos = i % columns;
                RectTransform child = rectChildren[i];
                child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
                SetChildAlongAxis(child, 0, colPos * actualCellSize + padding.left);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            if(rectChildren.Count <= 0) return;
            
            rows = (int) Mathf.Ceil(rectChildren.Count / (float) columns);
            float actualCellSize = cellSize.y + padding.horizontal;
            
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rows * actualCellSize);
            
            for (int i = 0; i < rectChildren.Count; i++)
            {
                int rowPos = Mathf.FloorToInt((float) i / columns);
                RectTransform child = rectChildren[i];
                child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y);
                SetChildAlongAxis(child, 1, rowPos * actualCellSize + padding.left);
            }
        }

        public override void SetLayoutHorizontal() { }
        public override void SetLayoutVertical() { }
    }
}
