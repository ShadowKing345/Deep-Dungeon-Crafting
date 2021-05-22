using System;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour
    {
        public Animator animator;
        public string[] idleNames;
        public string[] runningNames;
        public string attackName;
        private int _lastDirection;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Move(Vector2 direction)
        {
            string[] directions;

            if (direction.magnitude > 0.0f)
            {
                directions = runningNames;
                _lastDirection = DirectionIndex(direction);
            }
            else
            {
                directions = idleNames;
            }
            
            animator.Play(directions[_lastDirection]);
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

        private void PlayAnimation(string animationName, Action completionCallback)
        {
            try
            {
                animator.Play(animationName);
                completionCallback();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}
