using UnityEngine;

namespace Project.Runtime.Console
{
    //Todo: Implement Logging functions
    //Todo: Temporally store results of logs.
    //Todo: Ui Log viewer.
    //Todo: File base logs.
    public class LoggerBehaviour : MonoBehaviour
    {
        public static LoggerBehaviour Instance { get; private set; }

        [field: SerializeField] public bool EnableLogging { get; private set; } = true;

        private void Awake()
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

            Debug.Log("Testing", this);
        }

        public void Info(string message, Object obj)
        {
            if (!EnableLogging)
            {
                return;
            }

            Debug.Log(message, obj);
        }

        public void Warning()
        {
        }

        public void Error()
        {
        }
    }
}