using System;
using UnityEngine;
using Utils;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour
    {
        [Header("General Data")] [SerializeField]
        protected EntityData entityData;

        [SerializeField] protected Animator animator;

        [Header("State")] [SerializeField] protected Direction currentDirection;
        [SerializeField] protected State state;
        [SerializeField] protected AnimationClip attackAnimationClip;
        private float attackCoolDown;

        private bool isAttacking;
        private bool isEntityDataNull;

        public Direction CurrentDirection => currentDirection;

        public float GetAnimationLength => animator.GetCurrentAnimatorStateInfo(0).length;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            isEntityDataNull = entityData == null;
        }

        protected virtual void Update()
        {
            if (isEntityDataNull) return;

            if (isAttacking) state = State.Attacking;

            animator.Play((state switch
            {
                State.Idle => entityData.IdleAnimationData.GetDirection(currentDirection),
                State.Moving => entityData.MovingAnimationData.GetDirection(currentDirection),
                State.Attacking => attackAnimationClip,
                _ => throw new ArgumentOutOfRangeException()
            }).name);
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

        public void PlayAttackAnimation(AnimationClip clip)
        {
            attackAnimationClip = clip;
            attackCoolDown = Time.time + clip.length;
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