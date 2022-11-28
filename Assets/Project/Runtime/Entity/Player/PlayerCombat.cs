using System.Linq;
using Project.Runtime.Entity.Combat;
using Project.Runtime.Entity.Combat.Abilities;
using Project.Runtime.Managers;
using UnityEngine;

namespace Project.Runtime.Entity.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerCombat : MonoBehaviour
    {
        public delegate void WeaponClassChange(WeaponClass weaponClass);

        public PlayerEntity player;

        [Header("Class")] [SerializeField] private WeaponClass currentWeaponClass;

        [Space] private float actionCoolDown;

        public WeaponClassChange onChangeWeaponClass;
        private PlayerInventory playerInventory;
        private PlayerMovement playerMovement;

        public WeaponClass CurrentWeaponClass
        {
            get => currentWeaponClass;
            private set
            {
                currentWeaponClass = value ? value : GameManager.Instance.noWeaponClass;
                onChangeWeaponClass?.Invoke(currentWeaponClass);
            }
        }

        private void Start()
        {
            playerMovement = player.playerMovement;
            playerInventory = player.playerInventory;
            playerInventory.WeaponInventory.OnWeaponChanged += ChangeClass;

            CurrentWeaponClass = null;
        }

        private void OnDestroy()
        {
            playerInventory.WeaponInventory.OnWeaponChanged -= ChangeClass;
        }

        public void UseAbility(WeaponClass.AbilityIndex index)
        {
            if (Time.time <= actionCoolDown)
            {
                return;
            }

            var ability = currentWeaponClass.GetAbility(index).FirstOrDefault();

            if (!ability)
            {
                return;
            }

            if (!ability.CanExecute(player))
            {
                return;
            }

            if (ability is AreaOfEffectAbility aoeAbility)
            {
                aoeAbility.Execute(player, playerMovement.CurrentDirection);
            }
            actionCoolDown = Time.time + ability.Cooldown;
            playerMovement.PlayAttackAnimation(ability.AnimationData.GetDirection(playerMovement.CurrentDirection));
        }

        private void ChangeClass(WeaponClass weaponClass)
        {
            CurrentWeaponClass = weaponClass;
        }
    }
}