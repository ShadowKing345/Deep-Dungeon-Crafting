using System;
using UnityEngine;

namespace Combat
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
}