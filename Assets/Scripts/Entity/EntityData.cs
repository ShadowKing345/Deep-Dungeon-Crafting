using Entity.Animations;
using Entity.Combat;
using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(fileName = "New Entity Stats", menuName = "SO/Entity", order = 0)]
    public class EntityData : ScriptableObject
    {
        [Header("Information")] [SerializeField]
        private string description;

        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxMana = 50f;
        [Header("Combat")] [SerializeField] private AbilityProperty[] resistances;

        [Header("Model Data")] [SerializeField]
        private EntityAnimation idleAnimationData = new();

        [SerializeField] private EntityAnimation movingAnimationData = new();
        [SerializeField] private Vector2 centerPos;

        public string Description => description;
        public float MaxHealth => maxHealth;
        public float MaxMana => maxMana;
        public AbilityProperty[] Resistances => resistances;
        public EntityAnimation IdleAnimationData => idleAnimationData;
        public EntityAnimation MovingAnimationData => movingAnimationData;
        public Vector2 CenterPos => centerPos;
    }
}