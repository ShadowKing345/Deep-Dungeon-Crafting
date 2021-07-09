using System;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
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
        [SerializeField] private WeaponClass currentClass;
        [Space]
        [Header("Ability")]
        [SerializeField] private float weaponCoolDown;
        [Space]
        [Header("Combo")]
        [SerializeField] private WeaponClass.AbilityIndex comboAbilityIndex;
        [SerializeField] private int comboIndex;
        [SerializeField] private float comboCoolDown;
        
        private bool IsWeaponClassNull => currentClass == null;
        public bool IsEnabled { get; set; } = true;

        private void OnEnable()
        {
            inputManager ??= new InputManager();
            
            inputManager.Player.Enable();
            
            inputManager.Player.Ability1.canceled += ctx => ExecuteAbility(WeaponClass.AbilityIndex.Abilities1);
            inputManager.Player.Ability2.canceled += ctx => ExecuteAbility(WeaponClass.AbilityIndex.Abilities2);
            inputManager.Player.Ability3.canceled += ctx => ExecuteAbility(WeaponClass.AbilityIndex.Abilities3);
        }

        private void OnDisable() => inputManager.Player.Disable();

        private void Start()
        {
            _uiManager ??= UiManager.Instance;

            player ??= GetComponent<Player>();
            animator ??= GetComponent<EntityAnimator>();
            playerInventory ??= GetComponent<PlayerInventory>();

            if (playerInventory != null && playerInventory.WeaponInventory != null)
                playerInventory.WeaponInventory.OnWeaponChanged += ChangeWeapon;

            currentClass = GameManager.instance.noWeaponClass;
            ChangeWeapon(currentClass);
        }

        private void Update()
        {
            if (comboCoolDown > Time.time) return;
            comboIndex = 0;
        }

        public void ExecuteAbility(WeaponClass.AbilityIndex index)
        {
            if (!IsEnabled) return;
            if (weaponCoolDown > Time.time) return;
            Attack(index);
        }

        private void Attack(WeaponClass.AbilityIndex abilityIndex)
        {
            if (IsWeaponClassNull) return;

            if (abilityIndex != comboAbilityIndex)
            {
                _uiManager.SetAbilityUiComboIndex(comboAbilityIndex, -1);
                comboIndex = 0;
                comboAbilityIndex = abilityIndex;
            }

            Ability ability = currentClass.GetAbility(abilityIndex)[comboIndex];

            if (ability == null) return;

            player.ChargeMana(ability.ManaCost);

            if (ability.IsProjectile)
            {
                if (ability.ProjectilePreFab == null) return;

                Vector2 directionVector = GetAttackDirectionVector2(ability);

                GameObject projectile = Instantiate(ability.ProjectilePreFab, directionVector, Quaternion.identity);
                if (projectile.TryGetComponent(out IProjectile p)) p.Init(directionVector);

                return;
            }

            bool hitFlag = false;

            weaponCoolDown = Time.time + ability.CoolDown;

            foreach (IDamageable entity in GetHitList(ability))
                if (entity.Damage(ability.Properties))
                    hitFlag = true;

            animator.PlayAttackAnimation(ability.AnimationName);

            if (hitFlag) return;
            
            comboIndex++;
            if (comboIndex >= currentClass.GetAbility(comboAbilityIndex).Length) comboIndex = 0;
            
            _uiManager.SetAbilityUiComboIndex(abilityIndex, comboIndex);
            _uiManager.SetAbilityUiCoolDown(abilityIndex, ability.CoolDown);
            
            if (comboIndex == 0) return;

            comboCoolDown = Time.time + 10f;
        }

        private IDamageable[] GetHitList(Ability ability)
        {
            List<IDamageable> hitList = new List<IDamageable>();

            Collider2D[] entityHitList = Physics2D.OverlapCircleAll(GetAttackDirectionVector2(ability), ability.Range);

            foreach (Collider2D hit in entityHitList)
            {
                if (hit.gameObject == gameObject) continue;

                if (hit.TryGetComponent(out IDamageable damageable))
                    hitList.Add(damageable);
            }

            return hitList.ToArray();
        }

        private void ChangeWeapon(WeaponClass @class)
        {
            currentClass = @class;
            if (IsWeaponClassNull) return;

            Ability[][] abilities = @class.Abilities.ToArray();

            _uiManager.InitializeAbilityUi(this, new Dictionary<WeaponClass.AbilityIndex, Ability[]>
            {
                {WeaponClass.AbilityIndex.Abilities1, abilities[0]},
                {WeaponClass.AbilityIndex.Abilities2, abilities[1]},
                {WeaponClass.AbilityIndex.Abilities3, abilities[2]}
            });
        }

        public void UpdateCurrentWeaponClass()
        {
            ChangeWeapon(currentClass);
        }

        public float abilityRange;
        public Vector2 attackPoint;
        public Direction direction;

        private void OnDrawGizmosSelected()
        {
            if (player == null) return;
            if (abilityRange <= 0) return;

            Gizmos.DrawWireSphere(
                (Vector2) transform.position + player.Stats.GetCenterPos + GetPosFromDirection(attackPoint, direction),
                abilityRange);
        }

        private void ClearCombo()
        {
            _uiManager.SetAbilityUiComboIndex(comboAbilityIndex, -1);
            comboIndex = 0;
            comboCoolDown = 0;
        }

        private static Vector2 GetPosFromDirection(Vector2 pos, Direction direction) =>
            direction switch
            {
                Direction.S => new Vector2(pos.y, pos.x) * -1,
                Direction.W => pos * -1,
                Direction.N => new Vector2(pos.y, pos.x),
                Direction.E => pos,
                _ => Vector2.zero
            };

        private Vector2 GetAttackDirectionVector2(Ability ability) => (Vector2) transform.position +
                                                                      player.Stats.GetCenterPos +
                                                                      GetPosFromDirection(ability.AttackPoint,
                                                                          animator.GetCurrentDirection);
    }
}