using UnityEngine;
using Weapons;

namespace Entity
{
    public class Entity : MonoBehaviour, IDamageable
    {
        // Entity Stats
        public EntityStats stats;
        private int _health = 0;
        
        // Weapon Stats
        public WeaponClass weaponClass;
        public float weaponCooldown;

        // Hit layer masks
        public LayerMask hitLayers;

        public bool isDead;

        private void Awake()
        {
            _health = stats.maxHealth;
        }

        public bool Damage(int potency, WeaponElement element, WeaponAttackType attackType)
        {
            //Damage Animation

            //Take Damage
            _health -= potency;
            
            Debug.Log(potency);

            // Death check
            return true;
        }
    }
}