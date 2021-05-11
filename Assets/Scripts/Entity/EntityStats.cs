using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(fileName = "NewEntity", menuName = "SO/EntityStats", order = 0)]
    public class EntityStats : ScriptableObject
    {
        public int maxHealth = 100;
        public int attackPower = 10;
        public int defense = 10;
    }
}
