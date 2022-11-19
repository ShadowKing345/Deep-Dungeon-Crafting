using System;
using Combat;
using Combat.Buffs;
using UnityEngine.Events;

namespace Interfaces
{
    public interface IDamageable
    {
        bool Damage(AbilityProperty[] properties);
        bool Heal(float amount);
        bool Buff(BuffBase buffBase, float duration);
        void Die();

        float MaxHealth { get; }
        float CurrentHealth { get; }

        bool IsDead { get; }
    }
}