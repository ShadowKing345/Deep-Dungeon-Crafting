using Entity.Player;

namespace Items
{
    public abstract class UsableItem : Item
    {
        public abstract bool Use(PlayerEntity playerEntity);
    }
}