using UnityEngine;
using Weapons;

namespace Items
{
    [CreateAssetMenu(menuName = "SO/Weapon Item", fileName = "New Weapon Item")]
    public class WeaponItem : Item
    {
        public WeaponClass weaponClass;
        public WeaponProperty[] properties;
    }
}