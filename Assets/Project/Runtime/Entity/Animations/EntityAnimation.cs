using System;
using Project.Runtime.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Runtime.Entity.Animations
{
    [Serializable]
    public class EntityAnimation
    {
        [field: SerializeField] public bool UseDirection { get; set; }
        [field: SerializeField] public AnimationClip Clip { get; set; }
        [field: SerializeField] public AnimationClip North { get; set; }
        [field: SerializeField] public AnimationClip South { get; set; }
        [field: SerializeField] public AnimationClip East { get; set; }
        [field: SerializeField] public AnimationClip West { get; set; }

        public AnimationClip GetDirection(Direction currentDirection)
        {
            const string nameOf = nameof(currentDirection);

            return UseDirection
                ? currentDirection switch
                {
                    Direction.S => South,
                    Direction.E => East,
                    Direction.N => North,
                    Direction.W => West,
                    Direction.SW => throw new ArgumentOutOfRangeException(nameOf, currentDirection, null),
                    Direction.SE => throw new ArgumentOutOfRangeException(nameOf, currentDirection, null),
                    Direction.NW => throw new ArgumentOutOfRangeException(nameOf, currentDirection, null),
                    Direction.NE => throw new ArgumentOutOfRangeException(nameOf, currentDirection, null),
                    _ => throw new ArgumentOutOfRangeException(nameOf, currentDirection, null)
                }
                : Clip;
        }
    }
}