using Combat;
using Entity.Player;

namespace Interfaces
{
    public interface IAbility
    {
        string Name { get; }
        AbilityProperty[] Properties { get; }
        
        string AnimationName { get; }

        void Execute(Player player);
    }
}