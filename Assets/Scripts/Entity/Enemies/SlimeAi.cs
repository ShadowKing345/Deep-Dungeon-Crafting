using Combat;
using Interfaces;
using UnityEngine;

namespace Entity.Enemies
{
    public class SlimeAi : EntityAi
    {
        [SerializeField] private float windUpDelay = 3f;
        [SerializeField] private AbilityBase abilityBase;
        
        protected override void Attack()
        {
            if (Time.time < coolDown) return;
            isAttacking = true;
            
            Invoke(nameof(DoAttackCalculations), windUpDelay);
            
            animator.PlayAttackAnimation(abilityBase.AttackAnimationName);
            
            coolDown = Time.time + attackCoolDown + abilityBase.CoolDown + windUpDelay;
        }

        private void DoAttackCalculations()
        {
            Collider2D[] hitList = 
                Physics2D.OverlapCircleAll((Vector2) transform.position + abilityBase.AttackPoint, abilityBase.AttackRange);

            foreach (Collider2D hit in hitList)
            {
                if (hit.CompareTag("Enemy")) continue;
                
                if (hit.TryGetComponent(out IDamageable playerIDamageable))
                    if (!playerIDamageable.IsDead)
                        playerIDamageable.Damage(abilityBase.Properties);
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