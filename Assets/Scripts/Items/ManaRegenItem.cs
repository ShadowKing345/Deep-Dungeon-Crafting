using Entity.Player;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Mana Regen Item", menuName = "SO/Item/Mana Regen")]
    public class ManaRegenItem: UsableItem
    {
        [SerializeField] private float potency;

        public float Potency => potency;
        
        public override bool Use(Player player) => player.ChargeMana(-potency);
    }
}