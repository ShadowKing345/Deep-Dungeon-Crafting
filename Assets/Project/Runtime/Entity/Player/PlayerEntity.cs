using System.Linq;
using Project.Runtime.Entity.Combat;
using Project.Runtime.Entity.Combat.Buffs;
using Project.Runtime.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Runtime.Entity.Player
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerEntity : Entity
    {
        [field: Header("Player Entity Information")]
        [field: SerializeField]
        public PlayerInputManager InputManager { get; set; }

        [field: SerializeField] public PlayerMovement Movement { get; set; }
        [field: SerializeField] public PlayerInventory Inventory { get; set; }
        [field: SerializeField] public PlayerCombat Combat { get; set; }

        private void Awake()
        {
            if (InputManager == null)
            {
                InputManager = GetComponentInChildren<PlayerInputManager>();
                if (InputManager != null) InputManager.Entity = this;
            }

            if (Movement == null)
            {
                Movement = GetComponentInChildren<PlayerMovement>();
                if (Movement != null) Movement.Entity = this;
            }

            if (Inventory == null)
            {
                Inventory = GetComponentInChildren<PlayerInventory>();
                if (Inventory != null) Inventory.Entity = this;
            }

            // ReSharper disable once InvertIf
            if (Combat == null)
            {
                Combat = GetComponentInChildren<PlayerCombat>();
                if (Combat != null) Combat.Entity = this;
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
                    .Select(buff => ((DefensiveBuff)buff.buff).Properties).ToArray();

                var damage = buffResistance.SelectMany(abilityProperty => abilityProperty).Aggregate(property.Amount,
                    (current, property1) => current - current * property1.Amount);

                // Armor resistances
                var armorResistance = Inventory?.ArmorInventory.GetItemStacks()
                    .Where(stack =>
                        !stack.IsEmpty && stack.Item is ArmorItem ai && ai.properties.HasResistanceTo(property))
                    .Select(stack => ((ArmorItem)stack.Item).properties).ToArray() ?? null;

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