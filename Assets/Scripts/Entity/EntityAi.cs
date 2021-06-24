using System;
using Entity.Player;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity
{
    public abstract class EntityAi : MonoBehaviour
    {
        [SerializeField] protected Transform currentTarget;
        [SerializeField] protected State currentlyThinking;
        [SerializeField] protected float movementSpeed = 3f;
        [SerializeField] protected float agroRadius = 5f;
        [SerializeField] protected float attackRange = 1f;
        
        public Transform GetCurrentTarget => currentTarget;
        public State GetCurrentlyThinking => currentlyThinking;
        
        [SerializeField] private float nextWaypointDistance = 0.1f;

        private Path path;
        private int currentWaypoint;
        private bool reachedEndOfPath = true;
        private Vector2 direction;
        [SerializeField] protected float roamingDelay = 1.5f;
        protected float roamingCoolDown;

        [SerializeField] protected float attackCoolDown = 2.5f;
        protected bool isAttacking;
        protected float coolDown;
        
        private Seeker _seeker;
        private Rigidbody2D rb;
        protected EntityAnimator animator;

        protected void Start()
        {
            _seeker ??= GetComponent<Seeker>();
            rb ??= GetComponent<Rigidbody2D>();
            animator ??= GetComponent<EntityAnimator>();

            animator.IgnoreDirection = true;
            
            InvokeRepeating(nameof(Path), 0, 0.5f);
        }

        protected void Update()
        {
            if (isAttacking) return;
            
            Collider2D[] hitList = Physics2D.OverlapCircleAll(transform.position, agroRadius);
            Transform target = null;

            bool flag = false;

            foreach (Collider2D hit in hitList)
            {
                if (!hit.TryGetComponent<PlayerCombat>(out _)) continue;
                
                flag = true;
                target = hit.transform;
            }

            if (!flag)
            {
                ChangeState(State.Roaming);
            }
            else
            {
                ChangeState(State.Chasseing);
                currentTarget = target;
            }

            if (currentTarget == null) return;
            if (Vector2.Distance(currentTarget.position, transform.position) <= attackRange)
            {
                ChangeState(State.Attacking);
            }
        }

        protected void FixedUpdate()
        {
            switch (currentlyThinking)
            {
                case State.Idle:
                    break;
                case State.Roaming:
                case State.Chasseing:
                    Move();
                    break;
                case State.Attacking:
                    Attack();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void Path()
        {
            if (currentlyThinking == State.Roaming && !reachedEndOfPath || currentlyThinking == State.Idle || currentlyThinking == State.Attacking) return;

            var position = rb.position;
            _seeker.StartPath(position, currentlyThinking switch
            {
                State.Roaming => Random.insideUnitCircle * 2,
                State.Chasseing => currentTarget.position,
                _ => position
            }, p =>
            {
                if (p.error) return;

                path = p;
                currentWaypoint = 0;
            });
        }

        protected void Move()
        {
            direction = Vector2.zero;
            if (path == null) return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                if (currentlyThinking == State.Roaming) roamingCoolDown = Time.time + Random.Range(0, roamingDelay);
                return;
            }

            if (currentlyThinking == State.Roaming && Time.time < roamingCoolDown) return;

            reachedEndOfPath = false;

            var position = rb.position;
            direction = ((Vector2) path.vectorPath[currentWaypoint] - position).normalized;
            
            animator.Move(direction);
            
            rb.MovePosition(position + direction * ((currentlyThinking == State.Roaming ? movementSpeed / 2 : movementSpeed) * Time.fixedDeltaTime));
            
            if(Vector2.Distance(position, path.vectorPath[currentWaypoint]) < nextWaypointDistance) currentWaypoint++;
        }

        protected abstract void Attack();

        public void ChangeState(State state) => currentlyThinking = state;

        public enum State
        {
            Idle,
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