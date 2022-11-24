using System;
using Project.Runtime.Utils;
using UnityEngine;

namespace Project.Runtime.Items
{
    [CreateAssetMenu(menuName = "SO/Entity Drop Loot Table", fileName = "New Loot Table")]
    public class LootTable : ScriptableObject
    {
        [SerializeField] private Entry[] entries;
        [SerializeField] private MinMax<int> amount;

        public Entry[] Entries => entries;
        public MinMax<int> Amount => amount;

        [Serializable]
        public struct Entry
        {
            public Item item;
            public int weight;
        }
    }
}