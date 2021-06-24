using Interfaces;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Projectile : MonoBehaviour, IProjectile
    {
        public float movementSpeed;
        private Vector2 _direction;
    
        public float projectileLifespan;
        private float _startingTime;

        private Rigidbody2D _rb;
        private Collider2D _collider;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _startingTime = Time.time + projectileLifespan;
        }

        public void Init(Vector2 direction)
        {
            _direction = direction;
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _direction * (movementSpeed * Time.fixedDeltaTime));
            if (_startingTime <= Time.time)
            {
                Die();
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            Debug.Log("Yolo");
            Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}