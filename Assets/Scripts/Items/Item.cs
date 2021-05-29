using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "SO/Item", fileName = "New Item")]
    public class Item : ScriptableObject
    {
        public string description;
        public Sprite icon;
    }
}