using System;
using Project.Runtime.Utils;
using UnityEngine;

namespace Project.Runtime.Entity.Animations
{
    [Serializable]
    public class EntityAnimation
    {
        [SerializeField] private bool useDirection;
        [SerializeField] private AnimationClip clip;

        [SerializeField] private AnimationClip north;
        [SerializeField] private AnimationClip south;
        [SerializeField] private AnimationClip east;
        [SerializeField] private AnimationClip west;

        public bool UseDirection => useDirection;
        public AnimationClip Clip => clip;
        public AnimationClip North => north;
        public AnimationClip South => south;
        public AnimationClip East => east;
        public AnimationClip West => west;

        public AnimationClip GetDirection(Direction currentDirection)
        {
            return useDirection
                ? currentDirection switch
                {
                    Direction.S => south,
                    Direction.E => east,
                    Direction.N => north,
                    Direction.W => west,
                    Direction.SW => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection,
                        null),
                    Direction.SE => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection,
                        null),
                    Direction.NW => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection,
                        null),
                    Direction.NE => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection,
                        null),
                    _ => throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null)
                }
                : clip;
        }
    }
}