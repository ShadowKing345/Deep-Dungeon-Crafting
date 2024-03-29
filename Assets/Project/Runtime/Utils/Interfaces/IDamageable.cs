using Project.Runtime.Entity.Combat;

namespace Project.Runtime.Utils.Interfaces
{
    public interface IDamageable
    {
        float MaxHealth { get; }
        float CurrentHealth { get; }

        bool IsDead { get; }
        bool Damage(AbilityProperty[] properties);
        bool Heal(float amount);
        bool Buff(BuffBase buffBase, float duration);
        void Die();
    }
}