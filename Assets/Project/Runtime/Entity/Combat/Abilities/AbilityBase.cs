using UnityEngine;
using Utils.Interfaces;

namespace Entity.Combat.Abilities
{
    using Animations;

    public abstract class AbilityBase : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private string description;
        [SerializeField] private float cooldown = .5f;
        [SerializeField] private Sprite icon;
        [SerializeField] private EntityAnimation animationData = new();
        [SerializeField] private AbilityBase next;

        public string Name => name;
        public string Description => description;
        public float Cooldown => cooldown;
        public Sprite Icon => icon;
        public EntityAnimation AnimationData => animationData;
        public AbilityBase Next => next;

        public virtual bool Execute(IDamageable self)
        {
            return true;
        }

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
        Magical
    }
}