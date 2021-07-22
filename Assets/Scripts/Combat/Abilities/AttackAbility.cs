using Interfaces;
using UnityEngine;

namespace Combat.Abilities
{
    [CreateAssetMenu(fileName = "New Attack Ability", menuName = "SO/Combat/Ability/Attack Ability")]
    public class AttackAbility : AbilityBase
    {
        public override bool Execute(IDamageable self, IDamageable[] targets)
        {
            if(!base.Execute(self, targets)) return false;
            bool hitLanded = false;
            
            foreach (IDamageable damageable in targets)
                            if (damageable.Damage(Properties))
                                hitLanded = true;

            return hitLanded;
        }
    }
}