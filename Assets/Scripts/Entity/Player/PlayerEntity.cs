using System.Linq;
using Combat;
using Combat.Buffs;
using Items;
using UnityEngine;
using Utils.Event;
using Random = UnityEngine.Random;

namespace Entity.Player
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerEntity : Entity
    {
        public Player player;

        public override bool Damage(AbilityProperty[] properties)
        {
            float damageTaken = 0f;

            foreach (var property in properties)
            {
                // buff resistances
                var buffResistance = buffs
                    .Where(buff => buff.buff is DefensiveBuff dfb && dfb.Properties.HasResistanceTo(property))
                    .Select(buff => ((DefensiveBuff) buff.buff).Properties).ToArray();

                float damage = buffResistance.SelectMany(abilityProperty => abilityProperty).Aggregate(property.Amount,
                    (current, property1) => current - current * property1.Amount);

                // Armor resistances
                    var armorResistance = player.playerInventory?.ArmorInventory.GetItemStacks()
                        .Where(stack =>
                            !stack.IsEmpty && stack.Item is ArmorItem ai && ai.properties.HasResistanceTo(property))
                        .Select(stack => ((ArmorItem) stack.Item).properties).ToArray() ?? null;

                    damage = armorResistance.SelectMany(abilityProperty => abilityProperty).Aggregate(damage,
                        (current, property1) => current - current * property1.Amount);
                
                // natural resistances
                var resistance = stats.Resistances.FirstOrDefault(p =>
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
            
            // StatisticsManager.Instance.AddFloatValue("Player.Damage Taken", previousHealth - currentHealth);
            
            return true;
        }

        public void OnDamagePlayer()
        {
            onEntityEvent?.Invoke(new EntityEvent{Type = EntityEventType.Damage, Value = 0.5f});
        }
    }
}