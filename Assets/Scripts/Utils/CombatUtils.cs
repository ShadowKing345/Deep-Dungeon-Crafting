using System;
using System.Collections.Generic;
using Entity.Combat;
using Entity.Combat.Abilities;
using UnityEngine;
using Utils.Interfaces;

namespace Utils
{
    public static class CombatUtils
    {
        public static IDamageable[] GetHitList(AbilityBase abilityBase, Transform transform, Vector2 offSet,
            Direction direction)
        {
            var hitList = new List<IDamageable>();

            var entityHitList = Array.Empty<Collider2D>();
            // Physics2D.OverlapCircleAll(GetAttackDirection(abilityBase, transform, offSet, direction),
            // abilityBase.AttackRange);

            foreach (var hit in entityHitList)
            {
                if (hit.gameObject == transform.gameObject) continue;

                if (hit.TryGetComponent(out IDamageable damageable))
                    hitList.Add(damageable);
            }

            return hitList.ToArray();
        }

        public static Vector2 GetAttackDirection(AbilityBase abilityBase, Transform origin, Vector2 offset,
            Direction direction)
        {
            return Vector2.zero;
            //(Vector2) origin.position + offset + abilityBase.AttackPoint + direction.AsVector() * abilityBase.AttackDistance;
        }
    }
}