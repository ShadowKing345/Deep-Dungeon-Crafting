using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements;

namespace Utils.Ui
{
    public abstract class FollowMouse : MonoBehaviour
    { 
        [SerializeField] private RectTransform parentRT;
        [SerializeField] private RectTransform rt;
        [SerializeField] private new Camera camera;
        
        private void OnEnable() => Update();

        protected virtual void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, Mouse.current.position.ReadValue(), camera, out Vector2 position);

            position.x = Mathf.Clamp(position.x, 0, 1920 - rt.rect.width);
            position.y = Mathf.Clamp(position.y, -1080 + rt.rect.height, 0);
            
            rt.anchoredPosition = position;
        }
    }
}