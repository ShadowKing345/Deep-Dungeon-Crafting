using System;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour
    {
        private string[] _directions = {"South", "East", "North", "West"};
        
        public Animator animator;
        
        public string attackName;
        private float _attackDuration;

        public Vector2 direction;
        public bool isMoving;
        
        private int _lastDirection;
        [SerializeField] private State state;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isMoving)
            {
                state = State.Moving;
                _lastDirection = DirectionIndex(direction);
            }
            
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            switch (state)
            {
                case State.Idle:
                    animator.Play($"Idle-{_directions[_lastDirection]}");
                    break;
                
                case State.Moving:
                    animator.Play($"Run-{_directions[_lastDirection]}");
                    if (isMoving) return;
                    break;
                
                case State.Attacking:
                    animator.Play(attackName + $"-{_directions[_lastDirection]}");
                    
                    if (!(Time.time > _attackDuration) && !isMoving) return;

                    attackName = string.Empty;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

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

        public void PlayAttackAnimation(string attackName, float animationDuration, Action completionCallback)
        {
            this.attackName = attackName;
            state = State.Attacking;
            _attackDuration = Time.time + animationDuration;
        }

        public enum State
        {
            Idle,
            Moving,
            Attacking
        }
    }
}
