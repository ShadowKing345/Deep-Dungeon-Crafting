using UnityEngine;

namespace Ui.Journal
{
    [CreateAssetMenu(fileName = "New Journal Tab Collection", menuName = "SO/Journal/Tab Collection")]
    public class TabCollection : ScriptableObject
    {
        public Tab[] tabs;
    }
}