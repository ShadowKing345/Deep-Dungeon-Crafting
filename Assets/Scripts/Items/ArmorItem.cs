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
        Head,
        Body,
        Legs,
        Earing,
        Bracelet,
        Ring
    }
}