using Project.Runtime.Entity.Player;
using UnityEngine;

namespace Project.Runtime.Items
{
    [CreateAssetMenu(fileName = "New Healing Item", menuName = "SO/Item/Healing Item")]
    public class HealingItems : UsableItem
    {
        [SerializeField] private float potency;

        public float Potency => potency;

        public override bool Use(PlayerEntity playerEntity)
        {
            return playerEntity.Heal(potency);
        }
    }
}