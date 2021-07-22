using System;
using System.Linq;
using UnityEngine;
using Utils;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour
    {
        private readonly string[] _directions = {"South", "East", "North", "West"};
        
        [SerializeField] private Animator animator;
        [SerializeField] private string attackName;
        
        [SerializeField] private Direction currentDirection;
        [SerializeField] private State state = State.Idle;
        [SerializeField] private bool ignoreDirection;
        public bool IgnoreDirection { set => ignoreDirection = value; } 
        public Direction CurrentDirection => currentDirection;
        
        private float attackCoolDown;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            switch (state)
            {
                case State.Idle:
                    animator.Play(ignoreDirection ? "Idle" : $"Idle-{_directions[(int) currentDirection]}");
                    break;
                case State.Moving:
                    animator.Play(ignoreDirection ? "Run" : $"Run-{_directions[(int) currentDirection]}");
                    break;
                case State.Attacking:
                    animator.Play(ignoreDirection ? attackName : attackName + "-" + _directions[(int) currentDirection]);
                    if (attackCoolDown < Time.time) state = State.Idle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AttackComplete()
        {
            state = State.Idle;
        }

        public void Move(Vector2 direction)
        {
            if (state == State.Attacking) return;
            if (direction.magnitude > 0)
            {
                currentDirection = GetDirectionIndex(direction);
                state = State.Moving;
            }
            else
                state = State.Idle;
        }

        public static Direction GetDirectionIndex(Vector2 direction)
        {
            Vector2 normPos = direction.normalized;

            const float step = 90;
            const float offset = step / 2;
            float angle = Vector2.SignedAngle(Vector2.down, normPos);

            angle += offset;
            if (angle < 0) angle += 360;

            return (Direction) Mathf.FloorToInt(angle / step);
        }

        public void PlayAttackAnimation(string animationName)
        {
            attackName = animationName;
            state = State.Attacking;
            
            attackCoolDown = Time.time + GetAnimationLength(attackName);
        }

        public float GetAnimationLength(string animationName)
        {
            AnimationClip animationClip = animator.runtimeAnimatorController.animationClips.FirstOrDefault(clip =>
                clip.name == (ignoreDirection ? attackName : attackName + "-" + _directions[(int) currentDirection]));

            return animationClip != null ? animationClip.length : 0;
        }

        private enum State
        {
            Idle,
            Moving,
            Attacking
        }
    }
}
