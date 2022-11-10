using System.Collections;
using UnityEngine;

namespace Entity
{
    public abstract class EntityAiBase : MonoBehaviour
    {
        [SerializeField] protected Transform currentTarget;
        [SerializeField] protected State currentlyThinking;
        [SerializeField] protected float movementSpeed = 3f;
        [SerializeField] protected float agroRadius = 5f;
        [SerializeField] protected float attackRange = 1f;
        
        [Space]
        [SerializeField] private float nextWaypointDistance = 0.1f;
        [SerializeField] protected float roamingDelay = 1.5f;

        private int currentWaypoint;
        private Vector2 direction;
        private float roamingCoolDown;

        protected bool isAttacking;
        protected float attackCoolDown;

        private Rigidbody2D rb;
        protected EntityAnimator animator;

        public State GetCurrentlyThinking => currentlyThinking;

        protected void Start()
        {
            rb ??= GetComponent<Rigidbody2D>();
            animator ??= GetComponent<EntityAnimator>();

            currentlyThinking = State.Roaming;
            InvokeRepeating(nameof(Path), 0, 0.5f);
        }

        protected void Update()
        {
        }

        protected void Path()
        {
        }

        private void Move()
        {
        }

        private void FindTarget()
        {
        }

        protected abstract IEnumerator Attack(); 

        public void ChangeState(State state) => currentlyThinking = state;

        public enum State
        {
            Roaming,
            Chasseing,
            Attacking
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (agroRadius > 0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, agroRadius);
            }

            if (attackRange > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, attackRange);
            }
        }
    }
}