namespace Interfaces
{
    public interface IUsesMana
    {
        float MaxMana { get; }
        float CurrentMana { get; }

        bool ChargeMana(float amount);
    }
}