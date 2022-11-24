using Project.Runtime.Entity.Combat;
using UnityEngine;

namespace Project.Runtime.Items
{
    [CreateAssetMenu(menuName = "SO/Item/Armor Item", fileName = "New Armor Item")]
    public class ArmorItem : Item
    {
        public ArmorType type;
        public AbilityProperty[] properties;
    }

    public enum ArmorType
    {
        Head,
        Body,
        Legs,
        Earring,
        Bracelet,
        Ring
    }
}