using UnityEngine;
using Utils;
using Utils.Interfaces;

namespace Entity.Combat.Abilities
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
            Collider2D[] collisions = { };
            Physics2D.OverlapCircleNonAlloc(o, size.magnitude, collisions);

            var hit = false;
            foreach (var collision in collisions)
            {
                Debug.Log(collision);

                if (!collision.TryGetComponent(out IDamageable damageable)) continue;

                if (damageable.Damage(properties))
                {
                    hit = true;
                }
            }

            return hit;
        }
    }
}