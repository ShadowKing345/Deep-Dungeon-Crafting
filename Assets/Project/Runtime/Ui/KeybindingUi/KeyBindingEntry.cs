using Project.Runtime.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Project.Runtime.Ui.KeybindingUi
{
    public class KeyBindingEntry : MonoBehaviour
    {
        private static GameManager _gameManager;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI keyText;
        [SerializeField] private Image image;
        [SerializeField] private Button rebindButton;

        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

        private InputActionReference action;

        public InputActionReference Action
        {
            set
            {
                action = value;
                UpdateUi();
            }
        }

        private void OnEnable()
        {
            _gameManager ??= GameManager.Instance;
            rebindButton.onClick.AddListener(RebindKey);
        }

        private void UpdateUi()
        {
            if (action == null) return;

            nameText.text = action.name;

            UpdateBinding();
        }

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
            var currentControl = action.action.controls[0];
            var bindingIndex = action.action.GetBindingIndexForControl(action.action.controls[0]);

            var inputAction = action.action.bindings[bindingIndex];
        }
    }
}