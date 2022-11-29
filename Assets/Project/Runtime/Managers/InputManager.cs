using System;
using UnityEngine;

namespace Project.Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private Inputs.InputManager manager;
        public Inputs.InputManager Manager => manager ??= new Inputs.InputManager();

        [field: SerializeField] public Context InputContext { get; private set; } = Context.Player;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                return;
            }

            if (Instance == this)
            {
                return;
            }

            Destroy(this);
        }

        public void SwitchContext(Context context)
        {
            if (context == InputContext)
            {
                return;
            }

            if (manager.Player.enabled)
            {
                manager.Player.Disable();
            }

            if (manager.Debug.enabled)
            {
                manager.Debug.Disable();
            }

            if (manager.UI.enabled)
            {
                manager.UI.Disable();
            }

            switch (context)
            {
                case Context.Player:
                    manager.Player.Enable();
                    break;
                case Context.Debug:
                    manager.Debug.Enable();
                    break;
                case Context.Ui:
                    manager.UI.Enable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(context), context, "Context must be valid.");
            }

            InputContext = context;
        }

        public enum Context
        {
            Player,
            Debug,
            Ui
        }
    }
}