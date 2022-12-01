using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Runtime.Utils.Debug
{
    using Managers;

    public class DebugController : MonoBehaviour, Inputs.InputManager.IDebugActions
    {
        public static DebugController Instance { get; private set; }

        private DebugCommand testCommand;

        private GameManager gameManager;
        private InputManager inputManager;
        private bool showConsole;
        private InputManager.Context previousContext;
        private string input = "";

        private List<DebugCommandBase> commands;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else if (Instance != this)
            {
                Destroy(this);
                return;
            }


            testCommand = new DebugCommand("test", "test", "test", () =>
            {
                UnityEngine.Debug.Log("Test message", this);
                return true;
            });

            commands = new List<DebugCommandBase> {testCommand};
        }

        private void Start()
        {
            if (commands == null || commands.Count == 0)
            {
                enabled = false;
            }

            gameManager = GameManager.Instance;
            inputManager = gameManager.InputManager;

            inputManager.Manager.Debug.SetCallbacks(this);
        }

        public void OpenDebugMenu()
        {
            showConsole = true;
            previousContext = inputManager.InputContext;
            inputManager.SwitchContext(InputManager.Context.Debug);
        }

        private void CloseDebugMenu()
        {
            showConsole = false;
            inputManager.SwitchContext(previousContext);
        }

        private void Return()
        {
            if (string.IsNullOrEmpty(input.Trim()))
            {
                OpenDebugMenu();
                return;
            }

            var flag = false;
            foreach (var command in commands)
            {
                if (!command.Id.Contains(input)) continue;

                flag = command switch
                {
                    DebugCommand debugCommand => debugCommand.Invoke(),
                    _ => false
                };

                break;
            }

            if (flag)
            {
                CloseDebugMenu();
            }
        }

        private void OnGUI()
        {
            if (!showConsole)
            {
                return;
            }

            const float y = 0f;

            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = Color.black;
            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        }

        public void OnOpenDebugMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CloseDebugMenu();
            }
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Return();
            }
        }
    }
}