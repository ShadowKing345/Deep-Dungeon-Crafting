using System;
using System.Collections.Generic;
using Combat;
using Entity;
using Interfaces;
using UnityEngine;

namespace Utils
{
    public static class CombatUtils
    {
        public static IDamageable[] GetHitList(AbilityBase abilityBase, Transform transform, Vector2 offSet, Direction direction)
        {
            List<IDamageable> hitList = new List<IDamageable>();

            Collider2D[] entityHitList = Array.Empty<Collider2D>();
                // Physics2D.OverlapCircleAll(GetAttackDirection(abilityBase, transform, offSet, direction),
                    // abilityBase.AttackRange);

            foreach (Collider2D hit in entityHitList)
            {
                if (hit.gameObject == transform.gameObject) continue;

                if (hit.TryGetComponent(out IDamageable damageable))
                    hitList.Add(damageable);
            }

            return hitList.ToArray();
        }

        public static Vector2 GetAttackDirection(AbilityBase abilityBase, Transform origin, Vector2 offset,
            Direction direction) => Vector2.zero; //(Vector2) origin.position + offset + abilityBase.AttackPoint + direction.AsVector() * abilityBase.AttackDistance;
    }
}