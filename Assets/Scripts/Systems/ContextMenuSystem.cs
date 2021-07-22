using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems
{
    public class ContextMenuSystem : MonoBehaviour
    {
        private static ContextMenuSystem _instance;
        public static ContextMenuSystem Instance
        {
            get
            {
                _instance ??= FindObjectOfType<ContextMenuSystem>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value);
                    return;
                }

                _instance = value;
            }
        }

        [SerializeField] private Ui.ContextMenu.ContextMenu contextMenu;

        private void Awake() => Instance = this;

        public void OpenContextMenu(Dictionary<string, object> dictionary)
        {
            contextMenu.gameObject.SetActive(true);
            contextMenu.Setup = dictionary;
            contextMenu.SetPosition(Mouse.current.position.ReadValue());
        }

        public void HideContextMenu() => contextMenu.gameObject.SetActive(false);
    }
}