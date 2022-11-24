using Project.Runtime.Entity.Player;

namespace Project.Runtime.Items
{
    public abstract class UsableItem : Item
    {
        public abstract bool Use(PlayerEntity playerEntity);
    }
}