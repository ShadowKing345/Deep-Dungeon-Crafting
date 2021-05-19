using Weapons;

public interface IDamageable
{
    bool Damage(int potency, WeaponElement element, WeaponAttackType attackType);
}