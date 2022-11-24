using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ContextMenu = Project.Runtime.Ui.ContextMenu.ContextMenu;

namespace Project.Runtime.Systems
{
    public class ContextMenuSystem : MonoBehaviour
    {
        private static ContextMenuSystem _instance;

        [SerializeField] private ContextMenu contextMenu;

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

        private void Awake()
        {
            Instance = this;
        }

        public void OpenContextMenu(Dictionary<string, object> dictionary)
        {
            contextMenu.gameObject.SetActive(true);
            contextMenu.Setup = dictionary;
            contextMenu.SetPosition(Mouse.current.position.ReadValue());
        }

        public void HideContextMenu()
        {
            contextMenu.gameObject.SetActive(false);
        }
    }
}