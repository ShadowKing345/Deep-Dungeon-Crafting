using UnityEngine;

namespace Project.Runtime.Settings
{
    [CreateAssetMenu(fileName = "New Setting Asset", menuName = "SO/settingAsset")]
    public class SettingAsset : ScriptableObject
    {
        [SerializeField] private SettingPage[] pages;

        public SettingPage[] Pages => pages;
    }
}