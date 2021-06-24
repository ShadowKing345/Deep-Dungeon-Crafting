using UnityEngine;

namespace Ui.Journal
{
    [CreateAssetMenu(fileName = "New Journal Tab", menuName = "SO/Journal/Tab")]
    public class Tab : ScriptableObject
    {
        public Page page;
        public Tab[] subTabs;
    }
}