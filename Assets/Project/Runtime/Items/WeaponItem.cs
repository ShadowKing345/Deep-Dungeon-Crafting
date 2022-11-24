using Project.Runtime.Entity.Combat;
using UnityEngine;

namespace Project.Runtime.Items
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