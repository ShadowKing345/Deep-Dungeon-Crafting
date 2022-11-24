using UnityEngine;

namespace Project.Runtime.Utils.Interfaces
{
    public interface IAbility
    {
        string Name { get; }
        float CoolDown { get; }

        bool IsProjectile { get; }
        GameObject ProjectilePreFab { get; }

        string AnimationName { get; }

        Vector2 AttackPoint { get; }
        float AttackRange { get; }

        bool Execute(IDamageable self, IDamageable[] targets);
    }
}