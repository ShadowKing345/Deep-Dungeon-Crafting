using System;
using System.Collections.Generic;
using Combat;
using Interfaces;
using Items;
using Managers;
using Statistics;
using Ui.Notifications;
using UnityEngine;
using Utils;

namespace Entity.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCombat : MonoBehaviour
    {
        private UiManager _uiManager;
        private InputManager inputManager;

        [Header("Internal Components")]
        [SerializeField] private EntityAnimator animator;
        [SerializeField] private Player player;
        [SerializeField] private PlayerInventory playerInventory;
        [Space]
        [Header("Class")]
        [SerializeField] private WeaponClass currentClass;
        [Space]
        [SerializeField] private WeaponClass.AbilityIndex currentAbilityIndex = WeaponClass.AbilityIndex.None;
        [SerializeField] private int combo;

        [Space]
        private float actionCoolDown;
        private float comboCoolDown;
        [SerializeField] private bool isComboActive;

        private Ability[][] abilities;
        private bool IsWeaponClassNull => currentClass == null;
        
        private void OnEnable()
        {
            inputManager ??= new InputManager();
            
            inputManager.Player.Enable();
            
            inputManager.Player.Ability1.canceled += ctx => ExecuteAbility(WeaponClass.AbilityIndex.Abilities1);
            inputManager.Player.Ability2.canceled += ctx => ExecuteAbility(WeaponClass.AbilityIndex.Abilities2);
            inputManager.Player.Ability3.canceled += ctx => ExecuteAbility(WeaponClass.AbilityIndex.Abilities3);
            
            _uiManager ??= UiManager.Instance;

            player ??= GetComponent<Player>();
            animator ??= GetComponent<EntityAnimator>();
            playerInventory ??= GetComponent<PlayerInventory>();

            if (playerInventory == null || playerInventory.WeaponInventory == null) return;
                
            playerInventory.WeaponInventory.OnWeaponChanged += ChangeWeapon;

            ItemStack weaponSlot = playerInventory.WeaponInventory.GetStackAtSlot(0);

            currentClass = weaponSlot.IsEmpty ? GameManager.Instance.noWeaponClass : ((WeaponItem) playerInventory.WeaponInventory.GetStackAtSlot(0).Item).WeaponClass;
            ChangeWeapon(currentClass);
        }

        private void OnDisable() => inputManager.Player.Disable();

        private void Update()
        {
            if (comboCoolDown <= Time.time && isComboActive) ResetAllCooldown();
        }

        public void ExecuteAbility(WeaponClass.AbilityIndex index)
        {
            StatisticsManager.Instance.SetKeyValue("player.combat.action", Enum.GetName(typeof(WeaponClass.AbilityIndex), index));
            
            if (actionCoolDown > Time.time) return;
            
            if (currentAbilityIndex != index)
            {
                // NotificationSystem.Instance.Log("Ability Change. Resetting.");
                ResetAllCooldown();
                currentAbilityIndex = index;
            }

            if (abilities[(int) index].Length <= 0) return;
            
            if (combo >= abilities[(int) index].Length)
            {
                // NotificationSystem.Instance.Error("Combo out of bounds, resetting to safer state");
                ResetAllCooldown();
            }

            Ability ability = abilities[(int) index][combo];
            if (ability == null) return;
            
            _uiManager.SetAbilityUiCoolDown(index, ability.CoolDown);
            actionCoolDown = Time.time + ability.CoolDown;

            IDamageable[] hitList = CombatUtils.GetHitList(ability, player, animator.CurrentDirection);

            bool hitLanded = false;
            animator.PlayAttackAnimation(ability.AnimationName);

            foreach (IDamageable damageable in hitList)
                if (damageable.Damage(ability.Properties))
                    hitLanded = true;

            if (!hitLanded) return;

            combo++;
            if (combo >= abilities[(int) index].Length)
                ResetCooldown(index);

            comboCoolDown = Time.time + 5f;
            isComboActive = true;
            
            _uiManager.SetAbilityUiComboIndex(index, combo);
        }

        private void ChangeWeapon(WeaponClass @class)
        {
            currentClass = @class;
            if (IsWeaponClassNull)
            {
                Debug.LogWarning("Warning weapon class is null. Resetting to safe state.");
                currentClass = GameManager.Instance.noWeaponClass;
            };

            ResetAllCooldown();
            abilities = @class.Abilities;

            _uiManager.InitializeAbilityUi(this, new Dictionary<WeaponClass.AbilityIndex, Ability[]>
            {
                {WeaponClass.AbilityIndex.Abilities1, abilities[0]},
                {WeaponClass.AbilityIndex.Abilities2, abilities[1]},
                {WeaponClass.AbilityIndex.Abilities3, abilities[2]}
            });
        }

        private void ResetCooldown(WeaponClass.AbilityIndex index)
        {
            combo = 0;
            comboCoolDown = 0;
            currentAbilityIndex = WeaponClass.AbilityIndex.None;
            isComboActive = false;
            _uiManager.SetAbilityUiComboIndex(index, 0);
        }

        private void ResetAllCooldown()
        {
            ResetCooldown(WeaponClass.AbilityIndex.Abilities1);
            ResetCooldown(WeaponClass.AbilityIndex.Abilities2);
            ResetCooldown(WeaponClass.AbilityIndex.Abilities3);
        }

        public void UpdateCurrentWeaponClass() => ChangeWeapon(currentClass);

        [Space]
        [Header("Attack Point Gizmos Helper")]
        public float abilityRange;
        public Vector2 attackPoint;
        public Direction direction;

        private void OnDrawGizmosSelected()
        {
            if (player == null) return;
            if (abilityRange <= 0) return;

            Gizmos.DrawWireSphere(
                (Vector2) transform.position + player.Stats.GetCenterPos + CombatUtils.GetPosFromDirection(attackPoint, direction),
                abilityRange);
        }
    }
}