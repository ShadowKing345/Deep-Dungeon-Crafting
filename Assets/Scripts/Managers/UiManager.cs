using System.Collections.Generic;
using Enums;
using UnityEngine;
using Utils.Interfaces;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        private static UiManager _instance;

        [Space] [SerializeField] private WindowReference currentActive;

        private readonly Dictionary<WindowReference, GameObject> windowLookUp = new();

        private InputManager inputManager;

        public static UiManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<UiManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value);
                    return;
                }

                DontDestroyOnLoad(value);
                _instance = value;
            }
        }

        private void Awake()
        {
            Instance = this;
            SceneManager.Instance.OnEndSceneChange += _ => currentActive = WindowReference.None;
        }

        private void OnEnable()
        {
            inputManager ??= new InputManager();

            var windowsActions = inputManager.Windows;

            windowsActions.PauseMenu.canceled += _ => ToggleUiElement(WindowReference.PauseMenu);
            windowsActions.Crafting.canceled += _ => ToggleUiElement(WindowReference.CraftingMenu);
            windowsActions.PlayerInventory.canceled += _ => ToggleUiElement(WindowReference.PlayerInventory);

            windowsActions.Enable();
        }

        private void OnDisable()
        {
            inputManager.Windows.Disable();
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        #region Window Management

        public bool ToggleUiElement(WindowReference element)
        {
            if (element == WindowReference.PauseMenu && currentActive != WindowReference.PauseMenu &&
                currentActive != WindowReference.None && currentActive != WindowReference.Dialogue)
                return HideUiElement(currentActive);

            return IsUiElementShown(element) ? HideUiElement(element) : ShowUiElement(element);
        }

        public bool ShowUiElement(WindowReference element)
        {
            if (currentActive != WindowReference.None) return false;
            if (!TryGetWindow(element, out var windowObj)) return false;

            windowObj.SetActive(true);
            currentActive = element;

            if (windowObj.TryGetComponent(out IUiWindow uiWindow))
                uiWindow.Show();

            return true;
        }

        public bool HideUiElement(WindowReference element)
        {
            if (currentActive == WindowReference.None || currentActive != element) return false;
            if (!TryGetWindow(element, out var windowObj)) return false;

            if (windowObj.TryGetComponent(out IUiWindow uiWindow))
                uiWindow.Hide();

            windowObj.SetActive(false);
            currentActive = WindowReference.None;

            return true;
        }

        public void RegisterWindow(WindowReference element, GameObject obj)
        {
            if (windowLookUp.ContainsKey(element))
                windowLookUp[element] = obj;
            else
                windowLookUp.Add(element, obj);
        }

        public void UnregisterWindow(WindowReference element, GameObject obj)
        {
            if (windowLookUp.ContainsKey(element) && windowLookUp[element] == obj) windowLookUp.Remove(element);
        }

        private bool IsUiElementShown(WindowReference element)
        {
            return TryGetWindow(element, out var obj) && obj.activeSelf;
        }

        private bool TryGetWindow(WindowReference element, out GameObject obj)
        {
            return windowLookUp.TryGetValue(element, out obj);
        }

        #endregion
    }
}