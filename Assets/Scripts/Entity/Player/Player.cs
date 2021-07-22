using System;
using Combat;
using Managers;
using Statistics;

namespace Entity.Player
{
    public class Player : Entity
    {
        private UiManager _uiManager;

        public static event Action OnPlayerDeath;

        protected override void OnEnable()
        {
            base.OnEnable();
            _uiManager = UiManager.Instance;
        }

        private void Start() => _uiManager.HudElements.SetMaxHealthMana(stats.MaxHealth, stats.MaxMana);

        private void Update() => _uiManager.HudElements?.SetHealthMana(currentHealth, currentMana);

        public override bool Damage(AbilityProperty[] properties)
        {
            float previousHealth = currentHealth;
            
            if (!base.Damage(properties)) return false;
            
            StatisticsManager s = StatisticsManager.Instance;
            string path = "Player.Damage Taken";
            if (s.KeyExists(path))
            {
                switch (s.GetKeyValue(path))
                {
                    case int integer:
                        s.SetKeyValue(path, integer + (previousHealth - currentHealth));
                        break;
                    case string stringy:
                        if (int.TryParse(stringy, out int number))
                            s.SetKeyValue(path, (previousHealth - currentHealth));
                        break;
                }
            }
            else
                s.SetKeyValue(path, (previousHealth - currentHealth));

            return true;
        }

        public override bool Heal(float amount)
        {
            float previousHealth = currentHealth;
            
            if (!base.Heal(amount)) return false;
            
            StatisticsManager s = StatisticsManager.Instance;
            string path = "Player.Health Healed";
            if (s.KeyExists(path))
            {
                switch (s.GetKeyValue(path))
                {
                    case int integer:
                        s.SetKeyValue(path, integer + Math.Abs(previousHealth - currentHealth));
                        break;
                    case string stringy:
                        if (int.TryParse(stringy, out int number))
                            s.SetKeyValue(path, Math.Abs(previousHealth - currentHealth));
                        break;
                }
            }
            else
                s.SetKeyValue(path, Math.Abs(previousHealth - currentHealth));

            return true;
        }

        public override bool Buff(BuffBase buffBase, float duration)
        {
            if (!base.Buff(buffBase, duration)) return false;
            
            StatisticsManager s = StatisticsManager.Instance;
            string path = "Player.Buffed";
            if (s.KeyExists(path))
            {
                switch (s.GetKeyValue(path))
                {
                    case int integer:
                        s.SetKeyValue(path, integer + 1);
                        break;
                    case string stringy:
                        if (int.TryParse(stringy, out int number))
                            s.SetKeyValue(path, number + 1);
                        break;
                }
            }
            else
                s.SetKeyValue(path, 1);

            return true;
        }

        public override bool ChargeMana(float amount)
        {
            float previousMana = currentMana;
            
            if (!base.ChargeMana(amount)) return false;
            
            StatisticsManager s = StatisticsManager.Instance;
            string path = "Player.Mana Spent";
            if (s.KeyExists(path))
            {
                switch (s.GetKeyValue(path))
                {
                    case int integer:
                        s.SetKeyValue(path, integer + (previousMana - currentMana));
                        break;
                    case string stringy:
                        if (int.TryParse(stringy, out int number))
                            s.SetKeyValue(path, number + (previousMana - currentMana));
                        break;
                }
            }
            else
                s.SetKeyValue(path, (previousMana - currentMana));

            return true;
        }

        public override void Die()
        {
            StatisticsManager s = StatisticsManager.Instance;
            string path = "Player.Deaths";
            if (s.KeyExists(path))
            {
                switch (s.GetKeyValue(path))
                {
                    case int integer:
                        s.SetKeyValue(path, integer + 1);
                        break;
                    case string stringy:
                        if (int.TryParse(stringy, out int number))
                            s.SetKeyValue(path, number + 1);
                        break;
                }
            }
            else
                s.SetKeyValue(path, 1);
            
            OnPlayerDeath?.Invoke();
        }
    }
}