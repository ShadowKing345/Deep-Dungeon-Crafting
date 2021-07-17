using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ui.KeybindingUi
{
    public class KeyBindingEntry : MonoBehaviour
    {
        private static GameManager _gameManager;
        
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI keyText;
        [SerializeField] private Image image;
        [SerializeField] private Button rebindButton;

        private InputActionReference action;
        
        private void OnEnable()
        {
            _gameManager ??= GameManager.Instance;
            rebindButton.onClick.AddListener(RebindKey);
        }

        public InputActionReference Action
        {
            set
            {
                action = value;
                UpdateUi();
            }
        }

        private void UpdateUi()
        {
            if (action == null) return;

            nameText.text = action.name;
            
            UpdateBinding();
        }

        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

        private void RebindKey()
        {
            _rebindingOperation = action.action.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(_ => RebindComplete())
                .Start();
        }

        private void RebindComplete()
        {
            UpdateBinding();
            
            _rebindingOperation.Dispose();
        }

        private void UpdateBinding()
        {
            InputControl currentControl = action.action.controls[0];
            int bindingIndex = action.action.GetBindingIndexForControl(action.action.controls[0]);

            InputBinding inputAction = action.action.bindings[bindingIndex];
        }
    }
}