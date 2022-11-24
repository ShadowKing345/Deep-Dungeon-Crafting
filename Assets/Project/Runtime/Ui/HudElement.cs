using System;
using Project.Runtime.Entity.Combat;
using Project.Runtime.Entity.Player;
using Project.Runtime.Ui.HudElements;
using Project.Runtime.Utils;
using Project.Runtime.Utils.Event;
using TMPro;
using UnityEngine;

namespace Project.Runtime.Ui
{
    [Serializable]
    public class HudElement : MonoBehaviour
    {
        public PlayerEntity playerEntity;
        public PlayerCombat playerCombat;

        public ProgressBar healthProgressBar;
        public ProgressBar manaProgressBar;

        public AbilityUi abilityUi1;
        public AbilityUi abilityUi2;
        public AbilityUi abilityUi3;

        public TextMeshProUGUI floorNumber;

        public void OnEnable()
        {
            LoadOrder.init += Init;
        }

        private void OnDisable()
        {
            LoadOrder.init -= Init;
            if (playerEntity != null) playerEntity.onEntityEvent -= OnPlayerEntityEvent;
        }

        private void Init()
        {
            var player = FindObjectOfType<Player>();
            if (player != null)
            {
                playerEntity = player.playerEntity;
                playerCombat = player.playerCombat;
            }

            if (playerEntity != null)
            {
                playerEntity.onEntityEvent += OnPlayerEntityEvent;

                if (healthProgressBar != null)
                    healthProgressBar.Current = playerEntity.RelativeHealth * healthProgressBar.Max;

                if (manaProgressBar != null)
                    manaProgressBar.Current = playerEntity.RelativeMana * manaProgressBar.Max;
            }

            if (playerCombat == null) return;
            OnChangeWeaponClass(playerCombat.CurrentWeaponClass);
            playerCombat.onChangeWeaponClass += OnChangeWeaponClass;
        }

        private void OnChangeWeaponClass(WeaponClass weaponClass)
        {
            if (abilityUi1) abilityUi1.Abilities = weaponClass.GetAbility(WeaponClass.AbilityIndex.Abilities1);

            if (abilityUi2) abilityUi2.Abilities = weaponClass.GetAbility(WeaponClass.AbilityIndex.Abilities2);

            if (abilityUi3) abilityUi3.Abilities = weaponClass.GetAbility(WeaponClass.AbilityIndex.Abilities3);
        }

        private void OnPlayerEntityEvent(EntityEvent @event)
        {
            switch (@event.Type)
            {
                case EntityEventType.Damage:
                    healthProgressBar.Current = (float) (@event.Value * healthProgressBar.Max);
                    break;
                case EntityEventType.Death:
                case EntityEventType.Healing:
                case EntityEventType.Buff:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}