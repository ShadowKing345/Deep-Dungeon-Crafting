using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Entity.Combat.Abilities
{
    [CreateAssetMenu(fileName = "New Projectile Ability", menuName = "SO/Combat/Ability/Projectile Ability")]
    public class ProjectileAbility : AbilityBase
    {
        [SerializeField] private AbilityProperty[] properties;

        public AbilityProperty[] Properties => properties;

        public override bool Execute(IDamageable player)
        {
            return true;
        }
    }
}