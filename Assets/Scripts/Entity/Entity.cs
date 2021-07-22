using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using Combat;
using Combat.Buffs;
using Statistics;
using UnityEngine.Events;
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

        protected readonly List<ActiveBuff> buffs = new List<ActiveBuff>();
        
        public bool IsDead { get; protected set; }
        public event Action<IDamageable> OnDeath;

        private void Awake()
        {
            currentHealth = stats.MaxHealth;
            currentMana = stats.MaxMana;
        }

        protected virtual void OnEnable() => StartCoroutine(BuffTicks());

        protected virtual void OnDisable() => StopCoroutine(BuffTicks());

        public virtual bool Damage(AbilityProperty[] properties)
        {
            float damageTaken = 0f;

            foreach (AbilityProperty property in properties)
            {
                float damage = property.Amount;

                AbilityProperty resistance = stats.Resistances.FirstOrDefault(p =>
                    property.IsElemental ? p.Element == property.Element : p.AttackType == property.AttackType);

                damage -= property.Amount * resistance?.Amount ?? 0;

                AbilityProperty[][] buffResistance = buffs
                    .Where(buff => buff.buff is DefensiveBuff dfb && dfb.HasResistanceTo(property))
                    .Select(buff => ((DefensiveBuff) buff.buff).Properties).ToArray();

                damage = buffResistance.SelectMany(abilityProperties => abilityProperties)
                    .Aggregate(property.Amount, (current, p) => current - current * p.Amount);

                damageTaken += damage;
            }

            currentHealth -= Random.Range(0, 100) <= 30 ? damageTaken * 2 : damageTaken;

            if (!(currentHealth <= 0)) return true;

            if (godMode) return true;
            
            IsDead = true;
            Die();

            return true;
        }

        public virtual bool Heal(float amount)
        {
            if (!(currentHealth < stats.MaxHealth)) return false;
            
            currentHealth += currentHealth <= stats.MaxHealth ? amount : stats.MaxHealth - currentHealth;
            return true;
        }

        public virtual bool Buff(BuffBase buffBase, float duration)
        {
            buffs.Add(new ActiveBuff{buff = buffBase, duration = Time.time + duration});
            return true;
        }

        public virtual void Die()
        {
            OnDeath?.Invoke(this);
            
            StatisticsManager s = StatisticsManager.Instance;
            string path = $"Entity.Killed.{stats.name}";

            if (s.KeyExists(path))
            {
                switch (s.GetKeyValue(path))
                {
                    case int integer:
                        s.SetKeyValue(path, integer + 1);
                        break;
                    case string stringy:
                        if (int.TryParse(stringy, out int number))
                            s.SetKeyValue(path, number + 1);
                        break;
                }
            }
            else
                s.SetKeyValue(path, 1);
            
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
            if (!(currentMana >= amount)) return false;
            
            currentMana -= amount;
            return true;
        }

        public float GetMaxHealth() => stats.MaxHealth;
        public float GetCurrentHealth() => currentHealth;

        public float GetMaxMana => stats.MaxMana;
        public float GetCurrentMana => currentMana;
    }
}