using TMPro;
using UnityEngine;

namespace Project.Runtime.Console
{
    public class ConsoleBehaviour : MonoBehaviour
    {
        public static ConsoleBehaviour Instance { get; private set; }

        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_InputField textField;

        private ConsoleController controller;
        public ConsoleController Controller => controller ??= new ConsoleController();

        public void Awake()
        {
            if (canvas == null)
            {
                enabled = false;
                return;
            }

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

            Controller.AddCommand(new ConsoleCommand("test", "Testing Command", _ =>
            {
                Debug.Log("Testing", this);
                return true;
            }));
        }

        public void OpenDebugMenu()
        {
            canvas.gameObject.SetActive(true);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void CloseDebugMenu()
        {
            textField.text = "";
            canvas.gameObject.SetActive(false);
        }

        public void Return()
        {
            if (!Controller.ProcessCommand(textField.text)) return;

            CloseDebugMenu();
        }
    }
}