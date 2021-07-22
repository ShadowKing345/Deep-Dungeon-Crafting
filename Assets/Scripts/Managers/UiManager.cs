using System.Collections.Generic;
using Enums;
using Interfaces;
using Ui;
using UnityEngine;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        private static UiManager _instance;
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

        private InputManager inputManager;

        [SerializeField] private HudElement hudElements;
        [Space]
        [SerializeField] private WindowReference currentActive;

        private readonly Dictionary<WindowReference, GameObject> windowLookUp = new Dictionary<WindowReference, GameObject>();
        public HudElement HudElements => hudElements;

        private void Awake() => Instance = this;

        private void OnEnable()
        {
            inputManager ??= new InputManager();

            InputManager.WindowsActions windowsActions = inputManager.Windows;
            
            windowsActions.PauseMenu.canceled += _ => ToggleUiElement(WindowReference.PauseMenu);
            windowsActions.Crafting.canceled += _ => ToggleUiElement(WindowReference.CraftingMenu);
            windowsActions.PlayerInventory.canceled += _ => ToggleUiElement(WindowReference.PlayerInventory);

            windowsActions.Enable();
        }

        private void OnDisable() => inputManager.Windows.Disable();

        #region Hud

        public void SetUpHud(HudElement hudElement) => hudElements = hudElement;
        public void DestroyHud() => hudElements = null;
        
        #endregion

        #region Window Management

        public void ToggleUiElement(WindowReference element)
        {
            if (element == WindowReference.PauseMenu && currentActive != WindowReference.PauseMenu &&
                currentActive != WindowReference.None)
            {
                HideUiElement(currentActive);
                return;
            }

            if (IsUiElementShown(element)) HideUiElement(element);
            else ShowUiElement(element);
        }

        private void ShowUiElement(WindowReference element)
        {
            if (currentActive != WindowReference.None) return;
            if (!TryGetWindow(element, out GameObject windowObj)) return;
            
            windowObj.SetActive(true);
            currentActive = element;
            
            if (windowObj.TryGetComponent(out IUiWindow uiWindow))
                uiWindow.Show();
        }

        public void HideUiElement(WindowReference element)
        {
            if (currentActive == WindowReference.None || currentActive != element) return;
            if (!TryGetWindow(element, out GameObject windowObj)) return;

            windowObj.SetActive(false);
            currentActive = WindowReference.None;
            
            if (windowObj.TryGetComponent(out IUiWindow uiWindow))
                uiWindow.Hide();
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

        private bool IsUiElementShown(WindowReference element) => TryGetWindow(element, out GameObject obj) && obj.activeSelf;

        private bool TryGetWindow(WindowReference element, out GameObject obj) => windowLookUp.TryGetValue(element, out obj);

        #endregion
    }
}