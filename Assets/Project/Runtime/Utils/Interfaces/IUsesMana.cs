namespace Project.Runtime.Utils.Interfaces
{
    public interface IUsesMana
    {
        float MaxMana { get; }
        float CurrentMana { get; }

        bool Recharge(float amount);
    }
}