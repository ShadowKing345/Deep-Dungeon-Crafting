using System;
using System.Collections;
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

        private bool isAttacking;
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
                    if (isAttacking && attackCoolDown < Time.time)
                    {
                        state = State.Idle;
                        isAttacking = false;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        public IEnumerator PlayAttackAnimation(string animationName)
        {
            attackName = animationName;
            state = State.Attacking;
            
            animator.Play(ignoreDirection ? attackName : $"{attackName}-{_directions[(int) currentDirection]}");

            yield return new WaitForEndOfFrame();
            
            attackCoolDown = Time.time + GetAnimationLength;
            isAttacking = true;
        }

        public float GetAnimationLength => animator.GetCurrentAnimatorStateInfo(0).length;
        public float GetAnimationClipLength(string clipName)
        {
            AnimationClip clip =
                animator.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == clipName);
            return clip == null ? 0 : clip.length;
        }

        private enum State
        {
            Idle,
            Moving,
            Attacking
        }
    }
}
