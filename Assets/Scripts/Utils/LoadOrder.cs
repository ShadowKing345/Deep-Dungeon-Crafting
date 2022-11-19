using UnityEngine;

namespace Utils
{
    public class LoadOrder : MonoBehaviour
    {
        public delegate void LoadOrderEvent();
        
        public static LoadOrderEvent preInit;
        public static LoadOrderEvent init;
        public static LoadOrderEvent postInit;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}