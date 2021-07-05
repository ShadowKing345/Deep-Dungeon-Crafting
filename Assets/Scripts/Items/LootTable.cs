using System;
using UnityEngine;
using Utils;

namespace Items
{
    [CreateAssetMenu(menuName = "SO/Entity Drop Loot Table", fileName = "New Loot Table")]
    public class LootTable : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public Item item;
            public int weight;
        }

        [SerializeField] private Entry[] entries;
        [SerializeField] private MinMax<int> amount;

        public Entry[] Entries => entries;
        public MinMax<int> Amount => amount;
    }
}