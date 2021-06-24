using System.Collections.Generic;
using System.Linq;
using Combat;
using Interfaces;
using Managers;
using UnityEngine;
using Utils;

namespace Entity.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCombat : MonoBehaviour
    {
        private InputHandler inputHandler;
        private WindowManager windowManager;
        
        [SerializeField] private EntityAnimator animator;
        [SerializeField] private Player player;
        [SerializeField] private PlayerInventory playerInventory;
        
        [SerializeField] private WeaponClass weaponClass;
        
        private WeaponClass.AbilityIndex _abilityNumber;
        private Ability[] _actions;
        
        private int _actionComboIndex;
        
        private float _comboCoolDown;
        private bool _comboActive;

        private float _weaponCoolDown;
        private bool _isWeaponClassNull;
        public bool IsEnabled { get; set; } = true;

        private void Start()
        {
            windowManager ??= WindowManager.instance;
            inputHandler ??= InputHandler.instance;

            player ??= GetComponent<Player>();
            animator ??= GetComponent<EntityAnimator>();
            playerInventory ??= GetComponent<PlayerInventory>();
            
            if(playerInventory != null && playerInventory.weaponInventory != null)
                playerInventory.weaponInventory.OnWeaponChanged += ChangeWeapon;

            weaponClass = GameManager.instance.noWeaponClass;
        }
        
        private void Update()
        {
            if (!IsEnabled) return;
            if (!(Time.time >= _weaponCoolDown)) return;
            
            if (inputHandler.GetKeyDown(InputHandler.KeyValue.ExecuteAction1))
                Attack(WeaponClass.AbilityIndex.Abilities1);
            if (inputHandler.GetKeyDown(InputHandler.KeyValue.ExecuteAction2))
                Attack(WeaponClass.AbilityIndex.Abilities2);
            if (inputHandler.GetKeyDown(InputHandler.KeyValue.ExecuteAction3))
                Attack(WeaponClass.AbilityIndex.Abilities3);

            if (!(_comboCoolDown <= Time.time) || !_comboActive) return;

            _actionComboIndex = 0;
            _comboActive = false;
        }

        public void Attack(WeaponClass.AbilityIndex abilityIndex)
        {
            if (_isWeaponClassNull) return;

            Ability ability = weaponClass.GetAbility(abilityIndex).FirstOrDefault();

            if (ability == null) return;

            player.ChargeMana(ability.ManaCost);
            
            if (ability.IsProjectile)
            {
                if (ability.ProjectilePreFab == null) return;

                Vector2 directionVector = GetAttackDirectionVector2(ability);
                
                GameObject projectile = Instantiate(ability.ProjectilePreFab, directionVector, Quaternion.identity);
                projectile.GetComponent<IProjectile>().Init(directionVector);

                return;
            }

            // todo: implement combos.
            foreach (IDamageable entity in GetHitList(ability)) entity.Damage(ability.GetProperties);

            animator.PlayAttackAnimation(ability.AnimationName);
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
        
        private void ChangeWeapon(WeaponClass weaponClass)
        {
            _isWeaponClassNull = weaponClass == null;
            if (weaponClass == null)
                return;

            this.weaponClass = weaponClass;
            List<Ability[]> abilities = weaponClass.Abilities;

            
            windowManager.SetAbility(WeaponClass.AbilityIndex.Abilities1, abilities[0].FirstOrDefault());
            windowManager.SetAbility(WeaponClass.AbilityIndex.Abilities2, abilities[1].FirstOrDefault());
            windowManager.SetAbility(WeaponClass.AbilityIndex.Abilities3, abilities[2].FirstOrDefault());
        }

        public void UpdateCurrentWeaponClass()
        {
            ChangeWeapon(weaponClass);
        }

        public float abilityRange;
        public Vector2 attackPoint;
        public Direction direction;
        
        private void OnDrawGizmosSelected()
        {
            if (player == null) return;
            if (abilityRange <= 0) return;
            
            Gizmos.DrawWireSphere((Vector2) transform.position + player.Stats.GetCenterPos + GetPosFromDirection(attackPoint, direction), abilityRange);
        }

        private static Vector2 GetPosFromDirection(Vector2 pos, Direction direction) =>
            direction switch {
                Direction.S => new Vector2(pos.y, pos.x) * -1,
                Direction.W => pos * -1,
                Direction.N => new Vector2(pos.y, pos.x),
                Direction.E => pos,
                _ => Vector2.zero
            };
        
        private Vector2 GetAttackDirectionVector2(Ability ability) => (Vector2) transform.position + player.Stats.GetCenterPos + GetPosFromDirection(ability.AttackPoint, animator.GetCurrentDirection);
    }
}