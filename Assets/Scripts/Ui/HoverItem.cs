using System;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class HoverItem : MonoBehaviour
    {
        [SerializeField] private ItemStack stack;
        [SerializeField] private Image image;
        [SerializeField] private Canvas canvas;
        [SerializeField] private bool isReady;

        private void Awake()
        {
            image ??= GetComponent<Image>();
        }

        private void Start()
        {
            UpdatePos();
        }

        public void Init(ItemStack stack, Canvas canvas)
        {
            if (stack == null || canvas == null) return;
            this.stack = stack;
            this.canvas = canvas;

            if (stack.IsEmpty) return;
            
            image.sprite = stack.Item.icon;
            isReady = true;
        }

        private void Update()
        {
            if (!isReady) return;
            UpdatePos();
        }

        private void UpdatePos()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, canvas.worldCamera, out Vector2 pos);
            transform.position = canvas.transform.TransformPoint(pos);
        }
    }
}