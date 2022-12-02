using UnityEngine;

namespace Project.Runtime.Managers
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance { get; private set; }

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
        }
    }
}