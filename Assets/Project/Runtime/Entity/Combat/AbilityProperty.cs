using System;
using System.Linq;
using Project.Runtime.Entity.Combat.Abilities;
using UnityEngine;

namespace Project.Runtime.Entity.Combat
{
    [Serializable]
    public class AbilityProperty
    {
        [SerializeField] private bool isElemental;
        [SerializeField] private WeaponElement element;
        [SerializeField] private WeaponAttackType attackType;
        [SerializeField] private float amount;

        public bool IsElemental
        {
            get => isElemental;
            set => isElemental = value;
        }

        public WeaponElement Element
        {
            get => element;
            set => element = value;
        }

        public WeaponAttackType AttackType
        {
            get => attackType;
            set => attackType = value;
        }

        public float Amount
        {
            get => amount;
            set => amount = value;
        }
    }

    public static class Extension
    {
        public static AbilityProperty GetResistantTo(this AbilityProperty[] properties, AbilityProperty property)
        {
            return properties.FirstOrDefault(p =>
                p.IsElemental ? p.Element == property.Element : p.AttackType == property.AttackType);
        }

        public static bool HasResistanceTo(this AbilityProperty[] properties, AbilityProperty property)
        {
            return properties.FirstOrDefault(p =>
                p.IsElemental ? p.Element == property.Element : p.AttackType == property.AttackType) != null;
        }
    }
}