using Managers;
using UnityEngine;
using Weapons;

namespace Player
{
    public class PlayerStats : MonoBehaviour, IDamageable
    {
        private WindowManager _windowManager;
        
        public int maxHealth = 100;
        public int currentHealth;
        public int maxMana = 50;
        public int currentMana;

        public ProgressBar healthProgressBar;
        public ProgressBar manaProgressBar;

        private void Start()
        {
            _windowManager = WindowManager.instance;
            
            currentHealth = maxHealth;
            currentMana = maxMana;

            healthProgressBar = _windowManager.healthProgressBar;
            healthProgressBar.Setup(currentHealth, maxHealth);
            manaProgressBar = _windowManager.manaProgressBar;
            manaProgressBar.Setup(currentHealth, maxHealth);
        }

        public bool Damage(int potency, WeaponElement element, WeaponAttackType attackType)
        {
            currentHealth -= potency;
            return true;
        }

        private void Update()
        {
            healthProgressBar.current = currentHealth;
            manaProgressBar.current = currentMana;
        }
    }
}