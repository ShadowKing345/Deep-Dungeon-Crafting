using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Event;
using Utils.Interfaces;

namespace Entity
{
    using Combat;
    using Combat.Buffs;
    
    public class Entity : MonoBehaviour, IDamageable, IUsesMana
    {
        public delegate void EntityEvent(Utils.Event.EntityEvent @event);

        [SerializeField] protected EntityData data;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float currentMana;
        [Space] [SerializeField] protected bool godMode;

        protected readonly List<ActiveBuff> buffs = new();
        protected float damageCoolDown;
        protected bool damageCoolDownActive;

        public EntityEvent onEntityEvent;

        public EntityData Data => data;
        public float MaxHealth => data.MaxHealth;
        public float CurrentHealth => currentHealth;
        public float RelativeHealth => CurrentHealth / MaxHealth;
        public float MaxMana => data.MaxMana;
        public float CurrentMana => currentMana;
        public float RelativeMana => CurrentMana / MaxMana;
        public bool IsDead { get; protected set; }

        private void Awake()
        {
            currentHealth = data.MaxHealth;
            currentMana = data.MaxMana;
        }

        protected virtual void Update()
        {
            if (!damageCoolDownActive || !(damageCoolDown < Time.time)) return;

            damageCoolDownActive = false;
        }

        public virtual bool Damage(AbilityProperty[] properties)
        {
            var damageTaken = 0f;

            foreach (var property in properties)
            {
                var buffResistance = buffs
                    .Where(buff => buff.buff is DefensiveBuff dfb && dfb.Properties.HasResistanceTo(property))
                    .Select(buff => ((DefensiveBuff) buff.buff).Properties).ToArray();

                var damage = buffResistance.SelectMany(abilityProperties => abilityProperties)
                    .Aggregate(property.Amount, (current, p) => current - current * p.Amount);

                var resistance = data.Resistances.FirstOrDefault(p =>
                    property.IsElemental ? p.Element == property.Element : p.AttackType == property.AttackType);

                damage -= property.Amount * (resistance?.Amount ?? 0);

                damageTaken += damage;
            }

            currentHealth -= Mathf.Max(Random.Range(0, 100) <= 30 ? damageTaken * 2 : damageTaken, 0);

            damageCoolDownActive = true;
            damageCoolDown = Time.time + 0.6f;

            if (!(currentHealth <= 0)) return true;

            if (godMode) return true;

            onEntityEvent?.Invoke(new Utils.Event.EntityEvent {Type = EntityEventType.Damage});

            IsDead = true;
            Die();

            return true;
        }

        public virtual bool Heal(float amount)
        {
            if (!(currentHealth < data.MaxHealth)) return false;

            currentHealth += currentHealth <= data.MaxHealth ? amount : data.MaxHealth - currentHealth;
            onEntityEvent?.Invoke(new Utils.Event.EntityEvent {Type = EntityEventType.Healing});
            return true;
        }

        public virtual bool Buff(BuffBase buffBase, float duration)
        {
            buffs.Add(new ActiveBuff {buff = buffBase, duration = Time.time + duration});
            onEntityEvent?.Invoke(new Utils.Event.EntityEvent {Type = EntityEventType.Buff});
            return true;
        }

        public virtual void Die()
        {
            onEntityEvent?.Invoke(new Utils.Event.EntityEvent {Type = EntityEventType.Death});
            Destroy(gameObject);
        }

        public virtual bool Recharge(float amount)
        {
            if (currentMana < amount) return false;

            currentMana -= amount;
            return true;
        }
    }
}