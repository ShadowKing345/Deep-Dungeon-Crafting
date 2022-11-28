using Project.Runtime.Utils;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Entity.Combat.Abilities
{
    [CreateAssetMenu(fileName = "New Attack Ability", menuName = "SO/Combat/Ability/Attack Ability")]
    public class AreaOfEffectAbility : AbilityBase
    {
        [SerializeField] private AbilityProperty[] properties;
        [SerializeField] private Vector2 offset;
        [SerializeField] protected Vector2 size;

        public AbilityProperty[] Properties => properties;

        public bool Execute(IDamageable self, Direction facing)
        {
            if (self is not Entity entity)
            {
                return false;
            }

            var o = (Vector2) entity.transform.position + entity.Data.CenterPos + offset * facing.AsVector();

            var hit = false;
            // ReSharper disable once Unity.PreferNonAllocApi
            foreach (var collision in Physics2D.OverlapCircleAll(o, size.magnitude))
            {
                if (!collision.TryGetComponent(out IDamageable damageable)) continue;
                if (damageable == self)
                {
                    continue;
                }

                if (damageable.Damage(properties))
                {
                    hit = true;
                }
            }

            return hit;
        }
    }
}