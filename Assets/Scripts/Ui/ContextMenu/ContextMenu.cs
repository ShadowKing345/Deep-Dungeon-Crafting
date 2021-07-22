using System;
using System.Collections.Generic;
using Systems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils;

namespace Ui.ContextMenu
{
    public class ContextMenu : MonoBehaviour, IPointerExitHandler, IMoveHandler
    {
        [SerializeField] private RectTransform parentRT;
        [SerializeField] private RectTransform rt;
        [SerializeField] private new Camera camera;
        
        [Space]
        [SerializeField] private GameObject entryPreFab;
        [SerializeField] private GameObject spacerPreFab;
        [SerializeField] private Transform content;

        private void OnEnable() => SetPosition(Mouse.current.position.ReadValue());

        public Dictionary<string, object> Setup
        {
            set
            {
                GameObjectUtils.ClearChildren(content);

                foreach (KeyValuePair<string, object> kvPair in value)
                {
                    if ((kvPair.Value is Action
                        ? Instantiate(entryPreFab, content)
                        : Instantiate(spacerPreFab, content)).TryGetComponent(out ContextMenuEntry contextMenuEntry))
                        contextMenuEntry.SetUp(kvPair.Key, kvPair.Value as Action);
                }
            }
        }

        public void SetPosition(Vector2 position)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, position, camera, out Vector2 newPosition);

            newPosition.x = Mathf.Clamp(newPosition.x, 0, 1920 - rt.rect.width);
            newPosition.y = Mathf.Clamp(newPosition.y, -1080 + rt.rect.height, 0);
            
            rt.anchoredPosition = newPosition;
        }

        public void OnPointerExit(PointerEventData eventData) => ContextMenuSystem.Instance.HideContextMenu();
        public void OnMove(AxisEventData eventData) { }
    }
}