using UnityEngine;

namespace InGameHelp
{
    [CreateAssetMenu(fileName = "New Journal Page", menuName = "SO/Help/Page")]
    public class Page : ScriptableObject
    {
        public Sprite icon;
        [TextArea] public string description;
    }
}