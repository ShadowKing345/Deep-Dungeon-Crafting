using System;
using UnityEngine;

namespace Entity.Animations
{
    [Serializable]
    public class EntityAnimationClip
    {
        [SerializeField] private AnimationClip north;
        [SerializeField] private AnimationClip south;
        [SerializeField] private AnimationClip east;
        [SerializeField] private AnimationClip west;

        public AnimationClip North => north;
        public AnimationClip South => south;
        public AnimationClip East => east;
        public AnimationClip West => west;
    }
}