using UnityEngine;
using Weapons;

namespace Items
{
    [CreateAssetMenu(menuName = "SO/Armor Item", fileName = "New Armor Item")]
    public class ArmorItem : Item
    {
        public ArmorType type;
        public WeaponProperty[] properties;
    }

    public enum ArmorType
    {
        Head = 0,
        Body = 1,
        Legs = 2,
        Earring = 3,
        Bracelet = 4,
        Ring = 5
    }
}