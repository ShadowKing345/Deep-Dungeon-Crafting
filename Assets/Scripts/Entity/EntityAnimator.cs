using System;
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
        
        [SerializeField] private Direction lastDirection;
        [SerializeField] private State state = State.Idle;
        [SerializeField] private bool ignoreDirection;
        public bool IgnoreDirection { set => ignoreDirection = value; }
        public State GetCurrentState => state;
        public Direction GetCurrentDirection => lastDirection;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            switch (state)
            {
                case State.Idle:
                    animator.Play(ignoreDirection ? "Idle" : $"Idle-{_directions[(int) lastDirection]}");
                    break;
                case State.Moving:
                    animator.Play(ignoreDirection ? "Run" : $"Run-{_directions[(int) lastDirection]}");
                    break;
                case State.Attacking:
                    animator.Play(ignoreDirection ? attackName : attackName + "-" + _directions[(int) lastDirection]);
                    Invoke(nameof(AttackComplete), animator.GetCurrentAnimatorStateInfo(0).length);
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
            if (direction.magnitude > 0)
            {
                CancelInvoke(nameof(AttackComplete));
                lastDirection = GetDirectionIndex(direction);
                state = State.Moving;
            }
            else
            {
                if (state == State.Attacking) return;
                state = State.Idle;
            }
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
        }

        public enum State
        {
            Idle,
            Moving,
            Attacking
        }
    }
}
