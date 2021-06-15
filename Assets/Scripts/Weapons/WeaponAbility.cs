using System;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    public class WeaponAbility
    {
        public string name;
        [TextArea(1,10)]
        public string description;
        public Sprite icon;

        public int potency = 10;
        public float range = 0.5f;

        public bool isProjectile;
        public GameObject projectilePreFab;

        public float coolDown = 2.5f;
        
        public WeaponAttackType attackType;
        public WeaponElement elementType;
        public Vector2 attackPoint = new Vector2(0,0);
        
        public string animationName = string.Empty;

        public static WeaponAbility Empty { get; } = new WeaponAbility();
    }

    public enum WeaponElement
    {
        None,
        Water,
        Fire,
        Earth,
        Wind
    }

    public enum WeaponAttackType
    {
        Slashing,
        Piercing,
        Blunt,
        Magical,
    }
}