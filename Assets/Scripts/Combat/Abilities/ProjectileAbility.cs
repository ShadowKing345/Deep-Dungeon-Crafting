using Interfaces;
using UnityEngine;

namespace Combat.Abilities
{
    [CreateAssetMenu(fileName = "New Projectile Ability", menuName = "SO/Combat/Ability/Projectile Ability")]
    public class ProjectileAbility : AbilityBase
    {
        [SerializeField] private AbilityProperty[] properties;

        public AbilityProperty[] Properties => properties;

        public override bool Execute(IDamageable player, IDamageable[] targets)
        {
            var hitLanded = false;

            foreach (var damageable in targets)
            {
                if (damageable.Damage(properties))
                {
                    hitLanded = true;
                }
            }

            return hitLanded;
        }
    }
}