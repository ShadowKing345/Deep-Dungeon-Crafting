using UnityEngine;

namespace InGameHelp
{
    [CreateAssetMenu(fileName = "New Journal Tab", menuName = "SO/Journal/Tab")]
    public class Tab : ScriptableObject
    {
        public Page page;
        public Tab[] subTabs;
    }
}