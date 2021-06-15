using System;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour
    {
        private readonly string[] _directions = {"South", "East", "North", "West"};
        
        public Animator animator;
        
        public string attackName;

        public Vector2 direction;
        
        private int _lastDirection;
        [SerializeField] private State state = State.Idle;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            switch (state)
            {
                case State.Idle:
                    animator.Play($"Idle-{_directions[_lastDirection]}");
                    break;
                case State.Moving:
                    _lastDirection = DirectionIndex(direction);
                    animator.Play($"Run-{_directions[_lastDirection]}");
                    break;
                case State.Attacking:
                    animator.Play(attackName + "-" + _directions[_lastDirection]);
                    Invoke(nameof(AttackComplete), animator.GetCurrentAnimatorStateInfo(0).length);
                    break;
            }
        }

        private void AttackComplete()
        {
            state = State.Idle;
        }

        private static int DirectionIndex(Vector2 direction)
        {
            Vector2 normPos = direction.normalized;

            const float step = 90;
            const float offset = step / 2;
            float angle = Vector2.SignedAngle(Vector2.down, normPos);

            angle += offset;
            if (angle < 0)
            {
                angle += 360;
            }
            
            return Mathf.FloorToInt(angle / step);
        }

        public void ChangeState(State state)
        {
            this.state = state;
        }

        public enum State
        {
            Idle,
            Moving,
            Attacking
        }
    }
}
