using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using Combat;
using Combat.Buffs;
using Utils.Event;
using Random = UnityEngine.Random;

namespace Entity
{
    public class Entity : MonoBehaviour, IDamageable, IUsesMana
    {
        [SerializeField] protected EntityStats stats;
        public EntityStats Stats => stats;
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float currentMana;
        [Space] 
        [SerializeField] protected bool godMode;

        protected SpriteRenderer sp;
        protected float damageCoolDown;
        protected bool damageCoolDownActive;

        protected readonly List<ActiveBuff> buffs = new();
        
        public bool IsDead { get; protected set; }

        public delegate void OnEntityEvent(EntityEvent @event);

        public OnEntityEvent onEntityEvent;

        private void Awake()
        {
            currentHealth = stats.MaxHealth;
            currentMana = stats.MaxMana;
        }

        protected virtual void OnEnable()
        {
            sp = GetComponent<SpriteRenderer>();
            StartCoroutine(BuffTicks());
        }

        protected virtual void OnDisable() => StopCoroutine(BuffTicks());

        protected virtual void Update()
        {
            if (!damageCoolDownActive || !(damageCoolDown < Time.time)) return;
            
            sp.color = Color.white;
            damageCoolDownActive = false;
        }

        public virtual bool Damage(AbilityProperty[] properties)
        {
            float damageTaken = 0f;

            foreach (AbilityProperty property in properties)
            {
                AbilityProperty[][] buffResistance = buffs
                    .Where(buff => buff.buff is DefensiveBuff dfb && dfb.Properties.HasResistanceTo(property))
                    .Select(buff => ((DefensiveBuff) buff.buff).Properties).ToArray();

                float damage = buffResistance.SelectMany(abilityProperties => abilityProperties)
                    .Aggregate(property.Amount, (current, p) => current - current * p.Amount);
                
                AbilityProperty resistance = stats.Resistances.FirstOrDefault(p =>
                    property.IsElemental ? p.Element == property.Element : p.AttackType == property.AttackType);

                damage -= property.Amount * (resistance?.Amount ?? 0);

                damageTaken += damage;
            }

            currentHealth -= Mathf.Max(Random.Range(0, 100) <= 30 ? damageTaken * 2 : damageTaken, 0);

            damageCoolDownActive = true;
            damageCoolDown = Time.time + 0.6f;
            sp.color = Color.red;

            if (!(currentHealth <= 0)) return true;

            if (godMode) return true;
            
            onEntityEvent?.Invoke(new EntityEvent {Type = EntityEventType.Damage});

            IsDead = true;
            Die();
            
            return true;
        }

        public virtual bool Heal(float amount)
        {
            if (!(currentHealth < stats.MaxHealth)) return false;
            
            currentHealth += currentHealth <= stats.MaxHealth ? amount : stats.MaxHealth - currentHealth;
            onEntityEvent?.Invoke(new EntityEvent {Type = EntityEventType.Healing});
            return true;
        }

        public virtual bool Buff(BuffBase buffBase, float duration)
        {
            buffs.Add(new ActiveBuff{buff = buffBase, duration = Time.time + duration});
            onEntityEvent?.Invoke(new EntityEvent {Type = EntityEventType.Buff});
            return true;
        }

        public virtual void Die()
        {
            onEntityEvent?.Invoke(new EntityEvent{Type = EntityEventType.Death});
            Destroy(gameObject);
        }

        private IEnumerator BuffTicks()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                buffs.ForEach(buff => buff.buff.Tick());
                buffs.RemoveAll(buff => buff.duration < Time.time);
            }
        }

        public virtual bool ChargeMana(float amount)
        {
            if (currentMana < amount) return false;
            
            currentMana -= amount;
            return true;
        }

        public float MaxHealth => stats.MaxHealth;
        public float CurrentHealth => currentHealth;

        public float RelativeHealth => CurrentHealth / MaxHealth;

        public float MaxMana => stats.MaxMana;
        public float CurrentMana => currentMana;

        public float RelativeMana => CurrentMana / MaxMana;
    }
}