using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Runtime.Board
{
    [CreateAssetMenu(fileName = "new Floor Collection", menuName = "SO/Floor Collection")]
    public class FloorCollection : ScriptableObject
    {
        [SerializeField] private List<FloorItem> items;

        public List<FloorItem> Items => items;

        [Serializable]
        public struct FloorItem
        {
            public int levelIndex;
            public int completedFloorIndex;
            public FloorSettings settings;
        }
    }
}