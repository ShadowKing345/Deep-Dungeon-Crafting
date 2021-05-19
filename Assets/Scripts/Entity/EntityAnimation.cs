using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimation : MonoBehaviour
    {
        public Animator animator;
        public string[] idleAnimations;
        public string[] runningAnimations;

        private int _lastDirection;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void SetDirection(Vector2 direction)
        {
            string[] directions;

            if (direction.magnitude > 0.0f)
            {
                directions = runningAnimations;
                _lastDirection = DirectionIndex(direction);
            }
            else
            {
                directions = idleAnimations;
            }
            
            animator.Play(directions[_lastDirection]);
        }

        public static int DirectionIndex(Vector2 direction)
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
    }
}
