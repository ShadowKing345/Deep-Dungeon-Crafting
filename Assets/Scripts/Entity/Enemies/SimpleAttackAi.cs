using System.Collections;
using Combat;
using Interfaces;
using UnityEngine;

namespace Entity.Enemies
{
    public class SimpleAttackAi : EntityAiBase
    {
        [SerializeField] private AbilityBase abilityBase;

        protected override IEnumerator Attack()
        {
            if (isAttacking) yield break;
            isAttacking = true;

            yield return animator.PlayAttackAnimation(abilityBase.AttackAnimationName);

            float animationLenght = animator.GetAnimationLength;
            attackCoolDown = Time.time + Mathf.Max(animationLenght, abilityBase.CoolDown);

            yield return new WaitForSeconds(animator.GetAnimationLength);
            DoAttackCalculations();
        }

        private void DoAttackCalculations()
        {
            Collider2D[] hitList = 
                Physics2D.OverlapCircleAll((Vector2) transform.position + abilityBase.AttackPoint, abilityBase.AttackRange);

            foreach (Collider2D hit in hitList)
            {
                if (hit.CompareTag("Enemy")) continue;

                if (!hit.TryGetComponent(out IDamageable playerIDamageable)) continue;
                
                if (!playerIDamageable.IsDead)
                    playerIDamageable.Damage(abilityBase.Properties);
            }

            isAttacking = false;
        }
    }
}