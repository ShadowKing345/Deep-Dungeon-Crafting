using System;
using System.Linq;
using Combat;
using Combat.Buffs;
using Items;
using Managers;
using Statistics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity.Player
{
    public class PlayerEntity : Entity
    {
        private UiManager _uiManager;
        private PlayerInventory _playerInventory;

        public static event Action OnPlayerDeath;

        protected override void OnEnable()
        {
            base.OnEnable();
            _uiManager = UiManager.Instance;
            _playerInventory = GetComponent<PlayerInventory>();
        }

        private void Start() => _uiManager.HudElements.SetMaxHealthMana(stats.MaxHealth, stats.MaxMana);

        protected override void Update()
        {
            base.Update();
            _uiManager.HudElements?.SetHealthMana(currentHealth, currentMana);
        }

        public override bool Damage(AbilityProperty[] properties)
        {
            float previousHealth = currentHealth;
            
            float damageTaken = 0f;

            foreach (AbilityProperty property in properties)
            {
                // buff resistances
                AbilityProperty[][] buffResistance = buffs
                    .Where(buff => buff.buff is DefensiveBuff dfb && dfb.Properties.HasResistanceTo(property))
                    .Select(buff => ((DefensiveBuff) buff.buff).Properties).ToArray();

                float damage = buffResistance.SelectMany(abilityProperty => abilityProperty).Aggregate(property.Amount,
                    (current, property1) => current - current * property1.Amount);

                // Armor resistances
                if (_playerInventory != null)
                {
                    AbilityProperty[][] armorResistance = _playerInventory.ArmorInventory.GetItemStacks()
                        .Where(stack =>
                            !stack.IsEmpty && stack.Item is ArmorItem ai && ai.properties.HasResistanceTo(property))
                        .Select(stack => ((ArmorItem) stack.Item).properties).ToArray();

                    damage = armorResistance.SelectMany(abilityProperty => abilityProperty).Aggregate(damage,
                        (current, property1) => current - current * property1.Amount);
                }
                
                // natural resistances
                AbilityProperty resistance = stats.Resistances.FirstOrDefault(p =>
                    property.IsElemental ? p.Element == property.Element : p.AttackType == property.AttackType);

                damage -= damage * (resistance?.Amount ?? 0);


                damageTaken += damage;
            }

            currentHealth -= Random.Range(0, 100) <= 30 ? damageTaken * 2 : damageTaken;

            damageCoolDownActive = true;
            damageCoolDown = Time.time + 0.6f;
            sp.color = Color.red;

            if (!(currentHealth <= 0)) return true;

            if (godMode) return true;
            
            IsDead = true;
            Die();
            
            StatisticsManager.Instance.AddFloatValue("Player.Damage Taken", previousHealth - currentHealth);
            
            return true;
        }

        public override bool Heal(float amount)
        {
            float previousHealth = currentHealth;
            
            if (!base.Heal(amount)) return false;
            
            StatisticsManager.Instance.AddFloatValue("Player.Health Healed", currentHealth - previousHealth);

            return true;
        }

        public override bool Buff(BuffBase buffBase, float duration)
        {
            if (!base.Buff(buffBase, duration)) return false;
            
            StatisticsManager.Instance.AddIntValue("Player.Buffed", 1);

            return true;
        }

        public override bool ChargeMana(float amount)
        {
            float previousMana = currentMana;
            
            if (!base.ChargeMana(amount)) return false;
            
            StatisticsManager.Instance.AddFloatValue("Player.Mana Spent", previousMana - currentMana);

            return true;
        }

        public override void Die()
        {
            StatisticsManager.Instance.AddIntValue("Player.Deaths", 1);

            OnPlayerDeath?.Invoke();
        }
    }
}