using Combat;
using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(fileName = "New Entity Stats", menuName = "SO/Entity", order = 0)]
    public class EntityStats : ScriptableObject
    {
        [SerializeField] private string description;

        public string Description => description;

        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxMana = 50f;

        public float MaxHealth => maxHealth;
        public float MaxMana => maxMana;
        
        [SerializeField] private AbilityProperty[] resistances;

        public AbilityProperty[] Resistances => resistances;
        
        [SerializeField] private Vector2 centerPos;
        public Vector2 GetCenterPos => centerPos;
    }
}