using System;
using System.Linq;
using Interfaces;
using UnityEngine;
using Combat;
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
        
        public bool IsDead { get; private set; }
        public UnityEvent OnDeath { get; } = new UnityEvent();

        private void Awake()
        {
            currentHealth = stats.MaxHealth;
            currentMana = stats.MaxMana;
        }

        public virtual bool Damage(AbilityProperty[] properties)
        {
            float damageTaken = 0f;

            foreach (AbilityProperty property in properties)
            {
                AbilityProperty resistance = stats.Resistances.FirstOrDefault(i => property.IsElemental ? i.Element == property.Element : i.AttackType == property.AttackType);

                damageTaken += property.Amount - (resistance == null ? 0 : property.Amount * resistance.Amount);
            }

            currentHealth -= Random.Range(0, 100) <= 30 ? damageTaken * 2 : damageTaken;

            if (!(currentHealth <= 0)) return true;
            
            IsDead = true;
            Die();

            return true;
        }

        public virtual void Die()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }

        public bool ChargeMana(float amount)
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