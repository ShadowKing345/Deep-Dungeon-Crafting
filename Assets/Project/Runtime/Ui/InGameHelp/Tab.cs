using UnityEngine;

namespace Project.Runtime.Ui.InGameHelp
{
    [CreateAssetMenu(fileName = "New Journal Tab", menuName = "SO/Help/Tab")]
    public class Tab : ScriptableObject
    {
        public Page page;
        public Tab[] subTabs;
    }
}