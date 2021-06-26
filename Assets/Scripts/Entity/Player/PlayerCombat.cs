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
        private InputHandler _inputHandler;
        private WindowManager _windowManager;
        
        [SerializeField] private EntityAnimator animator;
        [SerializeField] private Player player;
        [SerializeField] private PlayerInventory playerInventory;
        
        [SerializeField] private WeaponClass weaponClass;
        private bool IsWeaponClassNull => weaponClass == null;
        
        private WeaponClass.AbilityIndex _comboAbilityIndex;
        private int _comboIndex;
        
        private float _comboCoolDown;

        private float _weaponCoolDown;
        
        public bool IsEnabled { get; set; } = true;

        private void Start()
        {
            _windowManager ??= WindowManager.instance;
            _inputHandler ??= InputHandler.instance;

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
            if (_weaponCoolDown < Time.time) return;
            
            if (_inputHandler.GetKeyDown(InputHandler.KeyValue.ExecuteAction1))
                Attack(WeaponClass.AbilityIndex.Abilities1);
            if (_inputHandler.GetKeyDown(InputHandler.KeyValue.ExecuteAction2))
                Attack(WeaponClass.AbilityIndex.Abilities2);
            if (_inputHandler.GetKeyDown(InputHandler.KeyValue.ExecuteAction3))
                Attack(WeaponClass.AbilityIndex.Abilities3);

            if (_comboCoolDown > Time.time) return;

            _comboIndex = 0;
        }

        public void Attack(WeaponClass.AbilityIndex abilityIndex)
        {
            if (IsWeaponClassNull) return;

            if (abilityIndex != _comboAbilityIndex)
            {
                _windowManager.SetAbility(_comboAbilityIndex, weaponClass.GetAbility(_comboAbilityIndex).FirstOrDefault());
                _comboIndex = 0;
            }
            Ability ability = weaponClass.GetAbility(abilityIndex)[_comboIndex];

            if (ability == null) return;

            player.ChargeMana(ability.ManaCost);
            
            if (ability.IsProjectile)
            {
                if (ability.ProjectilePreFab == null) return;

                Vector2 directionVector = GetAttackDirectionVector2(ability);
                
                GameObject projectile = Instantiate(ability.ProjectilePreFab, directionVector, Quaternion.identity);
                if(projectile.TryGetComponent(out IProjectile p)) p.Init(directionVector);

                return;
            }

            bool hitFlag = false;

            _weaponCoolDown = Time.time + ability.CoolDown;
            
            foreach (IDamageable entity in GetHitList(ability)) if(entity.Damage(ability.GetProperties)) hitFlag = true;

            animator.PlayAttackAnimation(ability.AnimationName);

            if (hitFlag) return;

            _comboAbilityIndex = abilityIndex;
            
            _comboIndex++;
            if (_comboIndex >= weaponClass.GetAbility(_comboAbilityIndex).Length) _comboIndex = 0;
            
            _windowManager.SetAbility(_comboAbilityIndex, weaponClass.GetAbility(_comboAbilityIndex)[_comboIndex], _comboIndex != 0);

            if (_comboIndex == 0) return;

            _comboCoolDown = Time.time + 10f;
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
            this.weaponClass = @class;
            if(IsWeaponClassNull) return;
            
            List<Ability[]> abilities = @class.Abilities;

            _windowManager.SetAbility(WeaponClass.AbilityIndex.Abilities1, abilities[0].FirstOrDefault());
            _windowManager.SetAbility(WeaponClass.AbilityIndex.Abilities2, abilities[1].FirstOrDefault());
            _windowManager.SetAbility(WeaponClass.AbilityIndex.Abilities3, abilities[2].FirstOrDefault());
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