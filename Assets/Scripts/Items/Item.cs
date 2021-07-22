using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    [CreateAssetMenu(menuName = "SO/Item/Item", fileName = "New Item")]
    public class Item : ScriptableObject
    {
        [Multiline]
        [SerializeField] protected string description;
        [SerializeField] protected Sprite icon;
        [Range(1, 999)]
        [SerializeField] protected int maxStackSize = 999;

        public string Description => string.IsNullOrEmpty(description) ? "No description." : description;
        public Sprite Icon => icon;
        public int MaxStackSize => maxStackSize;
    }
}