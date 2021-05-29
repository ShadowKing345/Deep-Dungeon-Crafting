using System.Collections.Generic;
using Entity;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCombat : MonoBehaviour
    {
        public PlayerMovement movementController;
        public PlayerAnimator animator;

        public Transform center;

        public WeaponClass weaponClass;
        
        private int _actionNumber;
        private WeaponAbility[] _actions;
        
        private int _actionComboIndex;
        
        private float _comboCoolDown;
        private bool _comboActive;

        private float _weaponCoolDown;
        private bool _isWeaponClassNull;

        private void Start()
        {
            _isWeaponClassNull = weaponClass == null;
            movementController ??= GetComponent<PlayerMovement>();
            animator ??= GetComponent<PlayerAnimator>();
            if (weaponClass != null)
                animator.bodyAnimator.animator.runtimeAnimatorController = weaponClass.animationController;
        }

        private void Update()
        {
            if (Time.time >= _weaponCoolDown)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    Attack(1);
                if (Input.GetKeyDown(KeyCode.Alpha2))
                    Attack(2);
                if (Input.GetKeyDown(KeyCode.Alpha3))
                    Attack(3);
            }

            if (_comboCoolDown <= Time.time && _comboActive)
            {
                _actionComboIndex = 0;
                _comboActive = false;
            }
        }

        private bool Attack(int actionIndex)
        {
            if (_isWeaponClassNull) return false;

            if (_actionNumber != actionIndex)
            {
                _actions = weaponClass.GetActionOfIndex(actionIndex);
                _actionNumber = actionIndex;
                _actionComboIndex = 0;
            }

            WeaponAbility ability = _actions[_actionComboIndex];

            animator.bodyAnimator.PlayAttackAnimation(ability.animationName, ability.coolDown, () => {Debug.Log("Attack Finished.");});

            if (!ability.isProjectile)
            {
                bool flag = false;
                
                foreach (IDamageable entity in GetHitList(ability))
                {
                    if (entity.Damage(ability.potency, ability.elementType, ability.attackType))
                        flag = true;
                }

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

            Collider2D[] entityHitList =
                Physics2D.OverlapCircleAll(
                    (Vector2) center.position + ability.attackPoint * movementController.Direction, ability.range);

            foreach (Collider2D entity in entityHitList) hitList.Add(entity.GetComponent<IDamageable>());

            return hitList.ToArray();
        }

        public void ChangeWeapon(WeaponClass weaponClass)
        {
            this.weaponClass = weaponClass;
            animator.bodyAnimator.animator.runtimeAnimatorController = weaponClass.animationController;
        }
    }
}