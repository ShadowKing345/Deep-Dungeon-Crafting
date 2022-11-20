using Interfaces;
using UnityEngine;

namespace Combat.Abilities
{
    [CreateAssetMenu(fileName = "New Attack Ability", menuName = "SO/Combat/Ability/Attack Ability")]
    public class AttackAbility : AbilityBase
    {
        [SerializeField] private AbilityProperty[] properties;
        
        public AbilityProperty[] Properties => properties;

        public override bool Execute(IDamageable self, IDamageable[] targets)
        {
            // if(!base.Execute(self, targets)) return false;
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