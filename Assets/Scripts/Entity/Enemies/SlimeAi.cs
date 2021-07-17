using Combat;
using Interfaces;
using UnityEngine;

namespace Entity.Enemies
{
    public class SlimeAi : EntityAi
    {
        [SerializeField] private float windUpDelay = 3f;
        [SerializeField] private Ability ability;
        
        protected override void Attack()
        {
            if (Time.time < coolDown) return;
            isAttacking = true;
            
            Invoke(nameof(DoAttackCalculations), windUpDelay);
            
            animator.PlayAttackAnimation(ability.AnimationName);
            
            coolDown = Time.time + attackCoolDown + ability.CoolDown + windUpDelay;
        }

        private void DoAttackCalculations()
        {
            Collider2D[] hitList = 
                Physics2D.OverlapCircleAll((Vector2) transform.position + ability.AttackPoint, ability.Range);

            foreach (Collider2D hit in hitList)
            {
                if (hit.CompareTag("Enemy")) continue;
                
                if (hit.TryGetComponent(out IDamageable playerIDamageable))
                    if (!playerIDamageable.IsDead)
                        playerIDamageable.Damage(ability.Properties);
            }

            isAttacking = false;
        }

        // protected override void OnDrawGizmosSelected()
        // {
        //     base.OnDrawGizmosSelected();
        //     
        //     if (ability?.Range <= 0) return;
        //
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawWireSphere(transform.position, ability.Range);
        // }
    }
}