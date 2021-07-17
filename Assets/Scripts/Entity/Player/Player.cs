using System;
using Managers;
using UnityEngine;

namespace Entity.Player
{
    public class Player : Entity
    {
        private UiManager _uiManager;
        private bool IsUiManagerNull => _uiManager == null;

        private void OnEnable() => _uiManager ??= UiManager.Instance;
        private void Start() => _uiManager.SetMaxHealthMana(stats.MaxHealth, stats.MaxMana);

        private void Update()
        {
            if (IsUiManagerNull) return;
            _uiManager.SetHealthMana(currentHealth, currentMana);
        }

        public override void Die()
        {
            Debug.Log("The player has died.");
        }
    }
}