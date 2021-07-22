using Interfaces;
using UnityEngine;

namespace Combat.Abilities
{    
    [CreateAssetMenu(fileName = "New Projectile Ability", menuName = "SO/Combat/Ability/Projectile Ability")]
    public class ProjectileAbility : AbilityBase
    {
        public override bool Execute(IDamageable player, IDamageable[] targets)
        {
            bool hitLanded = false;

            foreach (IDamageable damageable in targets)
            {
                if (damageable.Damage(Properties))
                    hitLanded = true;
            }

            return hitLanded;
        }
    }
}