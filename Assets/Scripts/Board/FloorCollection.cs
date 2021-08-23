using System;
using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    [CreateAssetMenu(fileName = "new Floor Collection", menuName = "SO/Floor Collection")]
    public class FloorCollection : ScriptableObject
    {
        [Serializable]
        public struct FloorItem
        {
            public int levelIndex;
            public int completedFloorIndex;
            public FloorSettings settings;
        }

        [SerializeField] private List<FloorItem> items;
        
        public List<FloorItem> Items => items;
    }
}