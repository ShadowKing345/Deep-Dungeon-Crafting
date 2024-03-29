using System;
using Project.Runtime.Entity.Animations;
using Project.Runtime.Utils;
using UnityEngine;

namespace Project.Runtime.Entity
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour
    {
        [Header("General Data")] [SerializeField]
        protected EntityData entityData;

        [SerializeField] protected Animator animator;

        [Header("State")] [SerializeField] protected Direction currentDirection;
        [SerializeField] protected State state;
        [SerializeField] protected EntityAnimation attackAnimation;
        private float attackCoolDown;

        private bool isAttacking;
        private bool isEntityDataNull;

        public Direction CurrentDirection => currentDirection;
        public float AttackAnimationClipLength => attackAnimation?.GetDirection(currentDirection)?.length ?? 0f;

        protected virtual void Awake()
        {
            if (entityData == null)
            {
                enabled = false;
                return;
            }

            animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            isEntityDataNull = entityData == null;
        }

        protected virtual void Update()
        {
            if (isEntityDataNull) return;

            if (isAttacking) state = State.Attacking;

            animator.Play((state switch
            {
                State.Idle => entityData.IdleAnimationData,
                State.Moving => entityData.MovingAnimationData,
                State.Attacking => attackAnimation,
                _ => throw new ArgumentOutOfRangeException()
            }).GetDirection(currentDirection).name);
        }

        protected virtual void FixedUpdate()
        {
            if (isAttacking && attackCoolDown < Time.time) isAttacking = false;
        }

        protected static Direction GetDirectionIndex(Vector2 direction)
        {
            var normPos = direction.normalized;

            const float step = 90f;
            const float offset = step / 2f;
            var angle = Vector2.SignedAngle(Vector2.down, normPos);

            angle += offset;
            if (angle < 0) angle += 360f;

            return (Direction) Mathf.FloorToInt(angle / step);
        }

        public void PlayAttackAnimation(EntityAnimation animationData)
        {
            attackAnimation = animationData;
            attackCoolDown = Time.time + animationData.GetDirection(currentDirection).length;
            isAttacking = true;
        }

        protected enum State
        {
            Idle,
            Moving,
            Attacking
        }
    }
}