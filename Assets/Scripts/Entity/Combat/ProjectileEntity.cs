using Entity.Combat.Abilities;
using UnityEngine;
using Utils;
using Utils.Interfaces;

namespace Entity.Combat
{
    public class ProjectileEntity : MonoBehaviour
    {
        [SerializeField] private AbilityBase abilityBase;

        [SerializeField] private float movementSpeed;
        [SerializeField] private Direction direction;

        [SerializeField] private float lifeTime;

        private Rigidbody2D rb;

        public IDamageable Caster { get; set; }

        public Direction Direction
        {
            get => direction;
            set => direction = value;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            Invoke(nameof(Die), lifeTime);
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + direction.AsVector() * movementSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Caster is MonoBehaviour mb && other.gameObject == mb.gameObject) return;

            abilityBase.Execute(Caster);
            Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}