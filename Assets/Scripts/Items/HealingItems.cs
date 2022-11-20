using Entity.Player;
using UnityEngine;

namespace Items
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