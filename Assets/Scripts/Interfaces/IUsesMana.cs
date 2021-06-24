namespace Interfaces
{
    public interface IUsesMana
    {
        float GetMaxMana { get; }
        float GetCurrentMana { get; }

        bool ChargeMana(float amount);
    }
}