using System;
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
        [field: SerializeField] public PlayerEntity Entity { get; set; }
        [Space] [SerializeField] private WeaponClass currentWeaponClass;

        public delegate void WeaponClassChange(WeaponClass weaponClass);
        public WeaponClassChange onChangeWeaponClass;

        private float actionCoolDown;
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
            playerMovement = Entity.Movement;
            playerInventory = Entity.Inventory;
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

            if (!ability.CanExecute(Entity))
            {
                return;
            }

            if (ability is AreaOfEffectAbility aoeAbility)
            {
                aoeAbility.Execute(Entity, playerMovement.CurrentDirection);
            }

            playerMovement.PlayAttackAnimation(ability.AnimationData);
            actionCoolDown = Time.time + Math.Max(ability.Cooldown, playerMovement.AttackAnimationClipLength);
        }

        private void ChangeClass(WeaponClass weaponClass)
        {
            CurrentWeaponClass = weaponClass;
        }
    }
}