using System.Collections.Generic;
using System.Linq;
using Entity;
using Interfaces;
using Managers;
using UnityEngine;
using Utils;
using Weapons;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCombat : MonoBehaviour
    {
        private WindowManager _windowManager;
        private InputHandler _inputHandler;
        
        public PlayerMovement movementController;
        public EntityAnimator animator;
        public PlayerStats playerStats;
        public PlayerInventory playerInventory;

        public bool isAttacking;

        public Transform center;

        public WeaponClass weaponClass;
        
        private int _actionNumber;
        private WeaponAbility[] _actions;
        
        private int _actionComboIndex;
        
        private float _comboCoolDown;
        private bool _comboActive;

        private float _weaponCoolDown;
        private bool _isWeaponClassNull;

        private void Awake()
        {
            _windowManager = WindowManager.instance;
            
            playerStats ??= GetComponent<PlayerStats>();
            movementController ??= GetComponent<PlayerMovement>();
            animator ??= GetComponent<EntityAnimator>();
            playerInventory ??= GetComponent<PlayerInventory>();
        }

        private void Start()
        {
            _isWeaponClassNull = weaponClass == null;
            _inputHandler = InputHandler.instance;

            playerInventory.weaponInventory.OnWeaponChanged += ChangeWeapon;
        }

        private void Update()
        {
            if (!(Time.time >= _weaponCoolDown)) return;

            if (_inputHandler.GetKeyDown(InputHandler.KeyValue.ExecuteAction1))
                Attack(1);
            if (_inputHandler.GetKeyDown(InputHandler.KeyValue.ExecuteAction2))
                Attack(2);
            if (_inputHandler.GetKeyDown(InputHandler.KeyValue.ExecuteAction3))
                Attack(3);

            if (!(_comboCoolDown <= Time.time) || !_comboActive) return;

            _actionComboIndex = 0;
            _comboActive = false;
        }

        private bool Attack(int actionIndex)
        {
            if (_isWeaponClassNull || isAttacking) return false;

            if (_actionNumber != actionIndex)
            {
                _actions = weaponClass.GetActionOfIndex(actionIndex);
                _actionNumber = actionIndex;
                _actionComboIndex = 0;
            }

            WeaponAbility ability = _actions[_actionComboIndex];

            isAttacking = true;
            
            // animator.bodyAnimator.PlayAttackAnimation(ability.animationName, ability.coolDown);

            if (!ability.isProjectile)
            {
                bool flag = false;
                
                foreach (IDamageable entity in GetHitList(ability))
                {
                    if (entity.Damage(ability.potency, ability.elementType, ability.attackType))
                        flag = true;
                }

                animator.attackName = ability.animationName;
                animator.ChangeState(EntityAnimator.State.Attacking);
                
                if (flag)
                {
                    _actionComboIndex++;
                    if (_actionComboIndex >= _actions.Length) _actionComboIndex = 0;
                    
                    _comboCoolDown = Time.time + 10;
                    _comboActive = true;
                    _weaponCoolDown = Time.time + ability.coolDown;
                    return true;
                }
            }
            else
            {
                if (ability.projectilePreFab == null) return false;
  
                GameObject projectile = Instantiate(ability.projectilePreFab, transform.position + (Vector3)(ability.attackPoint * movementController.Direction), Quaternion.identity);
                projectile.GetComponent<IProjectile>().Init(ability.attackPoint * movementController.Direction);
            }
            
            return false;
        }

        private IDamageable[] GetHitList(WeaponAbility ability)
        {
            List<IDamageable> hitList = new List<IDamageable>();

            Collider2D[] entityHitList = new Collider2D[0];
            for (int i = 0; i < Physics2D.OverlapCircleNonAlloc((Vector2) center.position + ability.attackPoint * movementController.Direction, ability.range, entityHitList); i++)
            {
                hitList.Add(entityHitList[i].GetComponent<IDamageable>());
            }
            
            return hitList.ToArray();
        }

        private void ChangeWeapon(WeaponClass weaponClass)
        {
            this.weaponClass = weaponClass;
            
            _windowManager.action1.SetAbility(weaponClass.action1.FirstOrDefault());
            _windowManager.action2.SetAbility(weaponClass.action2.FirstOrDefault());
            _windowManager.action3.SetAbility(weaponClass.action3.FirstOrDefault());
            
        }
    }
}