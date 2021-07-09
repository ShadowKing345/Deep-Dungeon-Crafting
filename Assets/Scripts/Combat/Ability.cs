using System;
using UnityEngine;

namespace Combat
{
    [Serializable]
    public class Ability
    {
        [SerializeField] private string name;
        [Multiline]
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;

        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;

        [SerializeField] private float coolDown = 2.5f;
        public float CoolDown => coolDown;

        [SerializeField] private AbilityProperty[] properties;
        [SerializeField] private float manaCost;
        public AbilityProperty[] Properties => properties;
        public float ManaCost => manaCost;

        [SerializeField] private bool isProjectile;
        [SerializeField] private GameObject projectilePreFab;

        public bool IsProjectile => isProjectile;
        public GameObject ProjectilePreFab => projectilePreFab;
        
        [SerializeField] private Vector2 attackPoint = new Vector2(0,0);
        [SerializeField] private float range = 0.5f;

        public Vector2 AttackPoint => attackPoint;
        public float Range => range;
        
        [SerializeField] private string animationName = string.Empty;

        public string AnimationName => animationName;

        public static Ability Empty { get; } = new Ability();
        public bool IsNull => Empty == this;
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