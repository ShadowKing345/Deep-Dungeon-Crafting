using System;
using Project.Runtime.Entity.Animations;
using Project.Runtime.Entity.Combat;
using UnityEngine;

namespace Project.Runtime.Entity
{
    [CreateAssetMenu(fileName = "New Entity Stats", menuName = "SO/Entity", order = 0)]
    public class EntityData : ScriptableObject
    {
        [SerializeField] private string description;
        [field: SerializeField] public float MovementSpeed { get; set; } = 7.5f;

        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxMana = 50f;
        [SerializeField] private AbilityProperty[] resistances;
        
        [SerializeField] private Vector2 centerPos;
        
        [SerializeField] private EntityAnimation idleAnimationData = new();
        [SerializeField] private EntityAnimation movingAnimationData = new();
        

        public string Description => description;
        public float MaxHealth => maxHealth;
        public float MaxMana => maxMana;
        public AbilityProperty[] Resistances => resistances;
        public EntityAnimation IdleAnimationData => idleAnimationData;
        public EntityAnimation MovingAnimationData => movingAnimationData;
        public Vector2 CenterPos => centerPos;
    }
}