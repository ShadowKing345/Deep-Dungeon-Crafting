using UnityEngine;

namespace Ui.Journal
{
    [CreateAssetMenu(fileName = "New Journal Page", menuName = "SO/Journal/Page")]
    public class Page : ScriptableObject
    {
        public Sprite icon;
        public string description;
    }
}