using System;
using UnityEngine;

namespace Project.Runtime.Utils
{
    [Serializable]
    public class MinMax<T>
    {
        [SerializeField] private T min;
        [SerializeField] private T max;

        public T Min
        {
            get => min;
            set => min = value;
        }

        public T Max
        {
            get => max;
            set => max = value;
        }
    }
}