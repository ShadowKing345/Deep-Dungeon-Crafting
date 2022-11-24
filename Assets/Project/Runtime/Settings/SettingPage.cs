using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class SettingPage
    {
        [SerializeField] private string name;
        [SerializeField] private Sprite image;
        [SerializeField] private SettingEntry[] settings;

        public string Name => name;
        public Sprite Image => image;

        public SettingEntry[] Settings
        {
            get => settings;
            set => settings = value;
        }
    }
}