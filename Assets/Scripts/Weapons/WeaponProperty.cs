using System;

namespace Weapons
{
    [Serializable]
    public class WeaponProperty
    {
        private bool _isElemental;
        private WeaponElement _element;
        private WeaponAttackType _attackType;
        private float _amount;

        public bool IsElemental
        {
            get => _isElemental;
            set => _isElemental = value;
        }

        public WeaponElement Element
        {
            get => _element;
            set => _element = value;
        }

        public WeaponAttackType AttackType
        {
            get => _attackType;
            set => _attackType = value;
        }

        public float Amount
        {
            get => _amount;
            set => _amount = value;
        }
    }
}