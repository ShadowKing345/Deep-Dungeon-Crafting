using System.Linq;
using Project.Runtime.Entity.Combat;
using Project.Runtime.Entity.Combat.Buffs;
using Project.Runtime.Items;
using UnityEngine;

namespace Project.Runtime.Entity.Player
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerEntity : Entity
    {
        public PlayerInputManager playerInputManager;
        public PlayerMovement playerMovement;
        public PlayerInventory playerInventory;
        public PlayerCombat playerCombat;

        private void Awake()
        {
            if (playerInputManager == null)
            {
                playerInputManager = GetComponentInChildren<PlayerInputManager>();
                if (playerInputManager != null) playerInputManager.player = this;
            }

            if (playerMovement == null)
            {
                playerMovement = GetComponentInChildren<PlayerMovement>();
                if (playerMovement != null) playerMovement.playerEntity = this;
            }

            if (playerInventory == null)
            {
                playerInventory = GetComponentInChildren<PlayerInventory>();
                if (playerInventory != null) playerInventory.player = this;
            }

            // ReSharper disable once InvertIf
            if (playerCombat == null)
            {
                playerCombat = GetComponentInChildren<PlayerCombat>();
                if (playerCombat != null) playerCombat.player = this;
            }
        }

        public override bool Damage(AbilityProperty[] properties)
        {
            var damageTaken = 0f;

            foreach (var property in properties)
            {
                // buff resistances
                var buffResistance = buffs
                    .Where(buff => buff.buff is DefensiveBuff dfb && dfb.Properties.HasResistanceTo(property))
                    .Select(buff => ((DefensiveBuff) buff.buff).Properties).ToArray();

                var damage = buffResistance.SelectMany(abilityProperty => abilityProperty).Aggregate(property.Amount,
                    (current, property1) => current - current * property1.Amount);

                // Armor resistances
                var armorResistance = playerInventory?.ArmorInventory.GetItemStacks()
                    .Where(stack =>
                        !stack.IsEmpty && stack.Item is ArmorItem ai && ai.properties.HasResistanceTo(property))
                    .Select(stack => ((ArmorItem) stack.Item).properties).ToArray() ?? null;

                damage = armorResistance.SelectMany(abilityProperty => abilityProperty).Aggregate(damage,
                    (current, property1) => current - current * property1.Amount);

                // natural resistances
                var resistance = data.Resistances.FirstOrDefault(p =>
                    property.IsElemental ? p.Element == property.Element : p.AttackType == property.AttackType);

                damage -= damage * (resistance?.Amount ?? 0);


                damageTaken += damage;
            }

            currentHealth -= Random.Range(0, 100) <= 30 ? damageTaken * 2 : damageTaken;

            damageCoolDownActive = true;
            damageCoolDown = Time.time + 0.6f;

            if (!(currentHealth <= 0)) return true;

            if (godMode) return true;

            IsDead = true;
            Die();

            return true;
        }
    }
}