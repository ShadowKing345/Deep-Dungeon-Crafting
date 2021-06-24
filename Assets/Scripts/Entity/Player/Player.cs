using Managers;
using UnityEngine;

namespace Entity.Player
{
    public class Player : Entity
    {
        private WindowManager windowManager;
        private bool IsWindowManagerNull => windowManager == null;
        
        private void Start()
        {
            windowManager ??= WindowManager.instance;
            windowManager.SetMaxHealthMana(stats.MaxHealth, stats.MaxMana);
        }

        private void Update()
        {
            if (IsWindowManagerNull) return;
            windowManager.SetHealthMana(currentHealth, currentMana);
        }

        public override void Die()
        {
            Debug.Log("The player has died.");
        }
    }
}