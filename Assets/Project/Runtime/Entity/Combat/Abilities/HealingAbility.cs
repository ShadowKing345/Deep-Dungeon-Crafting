using UnityEngine;
using Utils.Interfaces;

namespace Entity.Combat.Abilities
{
    [CreateAssetMenu(fileName = "New Healing Ability", menuName = "SO/Combat/Ability/Healing Ability")]
    public class HealingAbility : AbilityBase
    {
        [SerializeField] private float potency;
        public float Potency => potency;

        public override bool Execute(IDamageable self)
        {
            return base.Execute(self) && self.Heal(potency);
        }
    }
}