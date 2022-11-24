using System;
using UnityEngine.InputSystem;

namespace Project.Runtime.Settings
{
    [Serializable]
    public class KeyBindings
    {
        public string header;
        public InputActionReference[] actions;
    }
}