using System.Diagnostics.CodeAnalysis;
using Entity.Animations;
using Interfaces;
using UnityEngine;

namespace Combat
{
    public abstract class AbilityBase : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private AnimationClip animationClip;
        [SerializeField] private EntityAnimationClip entityAnimationClip = new EntityAnimationClip();
        [SerializeField] private AbilityBase next;

        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;
        public AnimationClip AnimationClip => animationClip;
        public AbilityBase Next => next;

        public abstract bool Execute(IDamageable self, IDamageable[] targets);

        public virtual bool CanExecute(IDamageable self)
        {
            return true;
        }
    }

    public enum WeaponElement
    {
        None,
        Water,
        Fire,
        Earth,
        Wind
    }

    public enum WeaponAttackType
    {
        Slashing,
        Piercing,
        Blunt,
        Magical,
    }
}