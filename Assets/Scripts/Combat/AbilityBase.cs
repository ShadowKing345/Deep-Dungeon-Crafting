using Interfaces;
using UnityEngine;

namespace Combat
{
    public abstract class AbilityBase : ScriptableObject, IAbility
    {
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
        [SerializeField] private float attackDistance = 0.5f;
        [SerializeField] private float attackRange = 0.5f;

        public Vector2 AttackPoint => attackPoint;
        public float AttackDistance => attackDistance;
        public float AttackRange => attackRange;
        
        [SerializeField] private string attackAnimationName = string.Empty;

        public string AttackAnimationName => attackAnimationName;

        public virtual bool Execute(IDamageable self, IDamageable[] targets)
        {
            if (self is Entity.Entity entity) return entity.ChargeMana(manaCost);
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