using System;
using UnityEngine;
using Utils;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour
    {
        private static readonly string[] Directions = {"South", "East", "North", "West"};

        [SerializeField] protected Animator animator;
        [SerializeField] protected string attackName;

        [SerializeField] protected Direction currentDirection;
        [SerializeField] protected State state;
        [SerializeField] protected bool ignoreDirection;

        private bool isAttacking;
        private float attackCoolDown;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        protected virtual void FixedUpdate()
        {
            if (isAttacking && attackCoolDown < Time.time)
            {
                isAttacking = false;
            }
        }

        protected virtual void Update()
        {
            if (isAttacking)
            {
                state = State.Attacking;
            }


            var animationName = state switch
            {
                State.Idle => "Idle",
                State.Moving => "Run",
                State.Attacking => attackName,
                _ => throw new ArgumentOutOfRangeException()
            };

            animator.Play(animationName + (ignoreDirection ? "" : "-" + Directions[(int) currentDirection]));
        }

        protected static Direction GetDirectionIndex(Vector2 direction)
        {
            var normPos = direction.normalized;

            const float step = 90;
            const float offset = step / 2;
            float angle = Vector2.SignedAngle(Vector2.down, normPos);

            angle += offset;
            if (angle < 0) angle += 360;

            return (Direction) Mathf.FloorToInt(angle / step);
        }

        public void PlayAttackAnimation(AnimationClip clip)
        {
            attackName = clip.name;
            attackCoolDown = Time.time + clip.length;
            isAttacking = true;
        }

        public float GetAnimationLength => animator.GetCurrentAnimatorStateInfo(0).length;

        protected enum State
        {
            Idle,
            Moving,
            Attacking
        }
    }
}