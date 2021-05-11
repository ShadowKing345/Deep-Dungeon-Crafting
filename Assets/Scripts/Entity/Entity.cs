using UnityEngine;
using Weapons;

namespace Entity
{
    public class Entity : MonoBehaviour
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
        
        public void Attack(WeaponAction action)
        {
            if (action == null) return;
            // Play animation
            
            // Detect enemies
            Collider2D[] entitiesHit = Physics2D.OverlapCircleAll((Vector2)transform.position + action.attackPoint, action.range, hitLayers);
            // Do damage
            foreach (Collider2D entity in entitiesHit)
            {
                //todo: there has got to be a better way to do this.
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                entity.GetComponent<Entity>().TakeDamage(action.potency);
            }
        }

        public void TakeDamage(int damageAmount)
        {
            //Damage Animation

            //Take Damage
            _health -= damageAmount;

            // Death check
            if (_health <= 0) Die();
        }

        public virtual void Die()
        {
            // Death Animation

            // Disable things
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Collider2D collider2D = GetComponent<Collider2D>();
            if (collider2D) collider2D.enabled = false;

            // Disable Script
            isDead = true;
            enabled = false;
            Destroy(gameObject);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (weaponClass == null) return;
            // Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}