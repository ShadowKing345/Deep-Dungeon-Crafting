using UnityEngine;

namespace InGameHelp
{
    [CreateAssetMenu(fileName = "New Journal Tab Collection", menuName = "SO/Help/Tab Collection")]
    public class TabCollection : ScriptableObject
    {
        public Tab[] tabs;
    }
}