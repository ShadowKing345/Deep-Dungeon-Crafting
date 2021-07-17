using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Settings
{
    [Serializable]
    public class KeyBindings
    {
        public string header;
        public InputActionReference[] actions;
    }
}