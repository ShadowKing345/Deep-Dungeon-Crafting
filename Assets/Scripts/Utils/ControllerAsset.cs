using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class ControllerAsset
    {
        [Serializable]
        public struct DirectionalCenter
        {
            public Sprite center;
            public Sprite north;
            public Sprite west;
            public Sprite south;
            public Sprite east;
        }
        
        [Serializable]
        public struct  ThreeStates
        {
            public Sprite off;
            public Sprite state1;
            public Sprite state2;
        }
        
        [Serializable]
        public struct Toggle
        {
            public Sprite off;
            public Sprite on;
        }

        [SerializeField] private DirectionalCenter dPad;
        [SerializeField] private DirectionalCenter buttonArray;
        [SerializeField] private ThreeStates leftThreeStates;
        [SerializeField] private ThreeStates rightThreeStates;

        [SerializeField] private Toggle leftAnalogue;
        [SerializeField] private Toggle rightAnalogue;

        [SerializeField] private Toggle home;
        [SerializeField] private ThreeStates startSelect;
    }
}