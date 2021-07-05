using Combat;
using UnityEngine.Events;

namespace Interfaces
{
    public interface IDamageable
    {
        bool Damage(AbilityProperty[] properties);
        void Die();
        
        float GetMaxHealth();
        float GetCurrentHealth();

        bool IsDead { get; }
        UnityEvent OnDeath { get; }
    }
}