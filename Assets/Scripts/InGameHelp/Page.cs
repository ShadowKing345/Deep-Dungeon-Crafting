using UnityEngine;

namespace InGameHelp
{
    [CreateAssetMenu(fileName = "New Journal Page", menuName = "SO/Journal/Page")]
    public class Page : ScriptableObject
    {
        public Sprite icon;
        public string description;
    }
}