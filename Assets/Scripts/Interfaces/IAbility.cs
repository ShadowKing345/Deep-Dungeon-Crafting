using UnityEngine;

namespace Interfaces
{
    public interface IAbility
    {
        string Name { get; }
        float CoolDown { get; }
        
        bool IsProjectile { get; }
        GameObject ProjectilePreFab { get; }
        
        string AttackAnimationName { get; }
        
        Vector2 AttackPoint { get; }
        float AttackRange { get; }
    
        bool Execute(IDamageable self, IDamageable[] targets);
    }
}