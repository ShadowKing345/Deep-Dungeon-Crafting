using UnityEngine;

namespace Project.Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private Inputs.InputManager manager;
        public Inputs.InputManager Manager => manager ??= new Inputs.InputManager();

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
    }
}