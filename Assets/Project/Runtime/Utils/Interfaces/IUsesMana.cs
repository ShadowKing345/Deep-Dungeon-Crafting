namespace Utils.Interfaces
{
    public interface IUsesMana
    {
        float MaxMana { get; }
        float CurrentMana { get; }

        bool Recharge(float amount);
    }
}