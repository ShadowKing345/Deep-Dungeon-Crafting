using UnityEngine;

namespace Project.Runtime.Ui.InGameHelp
{
    [CreateAssetMenu(fileName = "New Journal Tab Collection", menuName = "SO/Help/Tab Collection")]
    public class TabCollection : ScriptableObject
    {
        public Tab[] tabs;
    }
}