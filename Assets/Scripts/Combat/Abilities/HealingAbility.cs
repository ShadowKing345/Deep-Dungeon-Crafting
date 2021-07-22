using Interfaces;
using UnityEngine;

namespace Combat.Abilities
{
    [CreateAssetMenu(fileName = "New Healing Ability", menuName = "SO/Combat/Ability/Healing Ability")]
    public class HealingAbility : AbilityBase
    {
        [SerializeField] private float potency;
        public float Potency => potency;
        
        public override bool Execute(IDamageable self, IDamageable[] targets)
        {
            return base.Execute(self, targets) && self.Heal(potency);
        }
    }
}