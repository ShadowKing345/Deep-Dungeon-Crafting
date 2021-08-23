using System.Collections;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private Path path;
        private int currentWaypoint;
        private bool reachedEndOfPath = true;
        private Vector2 direction;
        private float roamingCoolDown;

        protected bool isAttacking;
        protected float attackCoolDown;

        private Seeker _seeker;
        private Rigidbody2D rb;
        protected EntityAnimator animator;

        public State GetCurrentlyThinking => currentlyThinking;

        protected void Start()
        {
            _seeker ??= GetComponent<Seeker>();
            rb ??= GetComponent<Rigidbody2D>();
            animator ??= GetComponent<EntityAnimator>();

            currentlyThinking = State.Roaming;
            InvokeRepeating(nameof(Path), 0, 0.5f);
        }

        protected void Update()
        {
            FindTarget();

            switch (currentlyThinking)
            {
                default:
                case State.Roaming:
                    if (roamingCoolDown < Time.time) Move();
                    if (reachedEndOfPath) roamingCoolDown = Time.time + Random.Range(0, roamingDelay);
                    break;
                case State.Chasseing:
                    Move();
                    break;
                case State.Attacking:
                    if (!isAttacking) Move();
                    if (attackCoolDown < Time.time) StartCoroutine(Attack());
                    break;
            }
        }

        protected void Path()
        {
            if (!reachedEndOfPath && currentlyThinking != State.Chasseing) return;

            var position = rb.position;
            _seeker.StartPath(position, currentlyThinking switch
            {
                State.Roaming => position + Random.insideUnitCircle * 2,
                State.Chasseing => currentTarget.position,
                _ => position
            }, p =>
            {
                if (p.error) return;
                if (!PathUtilities.IsPathPossible(p.path)) return;

                path = p;
                currentWaypoint = 0;
            });
        }

        private void Move()
        {
            direction = Vector2.zero;
            if (path == null) return;

            reachedEndOfPath = currentWaypoint >= path.vectorPath.Count;
            
            if(!reachedEndOfPath)
            {
                var position = rb.position;
                direction = ((Vector2) path.vectorPath[currentWaypoint] - position).normalized;
                
                rb.MovePosition(position + direction * (movementSpeed * Time.fixedDeltaTime));

                if (Vector2.Distance(position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
                    currentWaypoint++;
            }
            
            animator.Move(direction);
        }

        private void FindTarget()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, agroRadius);

            Transform target = null;

            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == gameObject) continue;
                if (hit.CompareTag("Player"))
                    target = hit.transform;
            }

            if (target == null)
            {
                currentlyThinking = State.Roaming;
                currentTarget = null;
            }
            else
            {
                currentlyThinking = Vector2.Distance(transform.position, target.transform.position) <= attackRange ? State.Attacking : State.Chasseing;
                currentTarget = target;
            }
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