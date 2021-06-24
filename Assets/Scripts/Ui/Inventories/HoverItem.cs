using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Inventories
{
    public class HoverItem : MonoBehaviour
    {
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
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition, 
                canvas.worldCamera,
                out var pos);
            transform.position = canvas.transform.TransformPoint(pos);
        }
    }
}