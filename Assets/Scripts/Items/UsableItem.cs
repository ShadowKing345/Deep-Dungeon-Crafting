using Combat;
using Entity.Player;
using UnityEngine;

namespace Items
{
    public abstract class UsableItem : Item
    {
        public abstract bool Use(Player player);
    }
}