using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ui.KeybindingUi
{
    public class KeyBindingEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI keyText;
        [SerializeField] private Button rebindButton;

        private InputActionReference action;
        
        private void OnEnable() => rebindButton.onClick.AddListener(RebindKey);

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
            int bindingIndex = action.action.GetBindingIndexForControl(action.action.controls[0]);
            keyText.text = action.action.bindings[bindingIndex].ToDisplayString();
        }
    }
}