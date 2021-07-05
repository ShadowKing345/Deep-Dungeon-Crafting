using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Utils.Ui
{
    public abstract class FollowMouse : MonoBehaviour
    {
        private void Start() => Update();

        protected virtual void Update()
        {
            Vector2 position = Mouse.current.position.ReadValue();

            if (TryGetComponent(out RectTransform rt))
                rt.pivot = new Vector2(position.x / Screen.width, position.y / Screen.height);

            transform.position = position;
        }
    }
}