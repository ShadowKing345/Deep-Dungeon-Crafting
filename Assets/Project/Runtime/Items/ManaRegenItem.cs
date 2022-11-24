using Project.Runtime.Entity.Player;
using UnityEngine;

namespace Project.Runtime.Items
{
    [CreateAssetMenu(fileName = "New Mana Regen Item", menuName = "SO/Item/Mana Regen")]
    public class ManaRegenItem : UsableItem
    {
        [SerializeField] private float potency;

        public float Potency => potency;

        public override bool Use(PlayerEntity playerEntity)
        {
            return playerEntity.Recharge(-potency);
        }
    }
}