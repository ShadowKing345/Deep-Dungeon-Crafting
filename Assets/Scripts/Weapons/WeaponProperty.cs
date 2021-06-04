using System;

namespace Weapons
{
    [Serializable]
    public class WeaponProperty
    {
        public bool isElemental;
        public WeaponElement element;
        public WeaponAttackType attackType;
        public float amount;
    }
}