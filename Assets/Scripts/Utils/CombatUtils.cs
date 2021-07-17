using System.Collections.Generic;
using Combat;
using Entity;
using Interfaces;
using UnityEngine;

namespace Utils
{
    public static class CombatUtils
    {
        public static IDamageable[] GetHitList(Ability ability, Entity.Entity entity, Direction direction)
        {
            List<IDamageable> hitList = new List<IDamageable>();

            Collider2D[] entityHitList =
                Physics2D.OverlapCircleAll(GetAttackDirection(ability, entity.transform, entity.Stats, direction),
                    ability.Range);

            foreach (Collider2D hit in entityHitList)
            {
                if (hit.gameObject == entity.gameObject) continue;

                if (hit.TryGetComponent(out IDamageable damageable))
                    hitList.Add(damageable);
            }

            return hitList.ToArray();
        }

        public static Vector2 GetAttackDirection(Ability ability, Transform origin, EntityStats entityStats,
            Direction direction) => (Vector2) origin.position + entityStats.GetCenterPos +
                                    GetPosFromDirection(ability.AttackPoint, direction);

        public static Vector2 GetPosFromDirection(Vector2 pos, Direction direction) =>
            direction switch
            {
                Direction.S => new Vector2(pos.y, pos.x) * -1,
                Direction.W => pos * -1,
                Direction.N => new Vector2(pos.y, pos.x),
                Direction.E => pos,
                _ => Vector2.zero
            };
    }
}