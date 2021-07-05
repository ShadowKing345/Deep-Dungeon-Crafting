using Combat;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "SO/Item/Weapon Item", fileName = "New Weapon Item")]
    public class WeaponItem : Item
    {
        [SerializeField] private WeaponClass weaponClass;
        [SerializeField] private AbilityProperty[] properties;

        public WeaponClass WeaponClass => weaponClass;
        public AbilityProperty[] Properties => properties;
    }
}