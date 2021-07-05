using UnityEngine;

namespace InGameHelp
{
    [CreateAssetMenu(fileName = "New Journal Tab Collection", menuName = "SO/Journal/Tab Collection")]
    public class TabCollection : ScriptableObject
    {
        public Tab[] tabs;
    }
}