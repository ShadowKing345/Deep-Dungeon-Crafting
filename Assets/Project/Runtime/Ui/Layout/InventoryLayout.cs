using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.Ui.Layout
{
    public class InventoryLayout : LayoutGroup
    {
        [Header("Cell Count Management")] [SerializeField]
        private int columns;

        [SerializeField] private int rows;
        [SerializeField] private bool swapColumnsRows;

        [Space] [Header("Cell Size Control")] [SerializeField]
        private Vector2 cellSize;

        [SerializeField] private bool fixedCellSize;
        [SerializeField] private bool squareCells;

        [Space] [Header("ParentControl")] [SerializeField]
        private bool setParentWidth;

        [SerializeField] private bool setParentHeight;

        public override void CalculateLayoutInputVertical()
        {
            if (!swapColumnsRows)
            {
                if (columns == 0) return;
                columns = Math.Abs(columns);

                rows = Mathf.CeilToInt((float) rectChildren.Count / columns);
            }
            else
            {
                if (rows == 0) return;
                rows = Math.Abs(rows);

                columns = Mathf.CeilToInt((float) rectChildren.Count / rows);
            }

            var rect = rectTransform.rect;

            if (!fixedCellSize)
            {
                if (squareCells)
                {
                    cellSize.x = cellSize.y = rect.width / columns;
                }
                else
                {
                    cellSize.x = rect.width / columns;
                    cellSize.y = rect.height / rows;
                }
            }

            if (setParentWidth)
            {
                var width = cellSize.x * columns;
                SetLayoutInputForAxis(width, width, -1, 0);
            }

            if (setParentHeight)
            {
                var height = cellSize.y * rows;
                SetLayoutInputForAxis(height, height, -1, 1);
            }

            for (var i = 0; i < rectChildren.Count; i++)
            {
                var child = rectChildren[i];
                var pos = new Vector2(i % columns, Mathf.Floor(i / (float) columns)) * cellSize;

                child.sizeDelta = cellSize;
                SetChildAlongAxis(child, 0, pos.x);
                SetChildAlongAxis(child, 1, pos.y);
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