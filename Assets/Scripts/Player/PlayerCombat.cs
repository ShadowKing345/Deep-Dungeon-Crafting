using System;
using System.Collections.Generic;
using Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.UIElements;
using Weapons;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCombat : MonoBehaviour
    {
        public WeaponClasses classes;
        private WeaponClass _weaponClass;
        
        private int _actionNumber;
        private WeaponAction[] _actions;
        
        private int _actionComboIndex;
        
        private float _comboCoolDown;
        private bool _comboActive;

        private float _weaponCoolDown;

        public PlayerMovement movementController;

        public Vector2 attackPoint;
        public float range;
        
        private void Start()
        {
            movementController = GetComponent<PlayerMovement>();
            _weaponClass = classes.gameData[0];
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
            if (_weaponClass == null) return false;

            if (_actionNumber != actionIndex)
            {
                _actions = _weaponClass.GetActionOfIndex(actionIndex);
                _actionNumber = actionIndex;
                _actionComboIndex = 0;
            }

            WeaponAction action = _actions[_actionComboIndex];

            if (!action.isProjectile)
            {
                bool flag = false;
                
                foreach (IDamageable entity in GetHitList(action))
                {
                    if (entity.Damage(action.potency, action.elementType, action.attackType))
                        flag = true;
                }

                if (flag)
                {
                    _actionComboIndex++;
                    if (_actionComboIndex >= _actions.Length) _actionComboIndex = 0;

                    _comboCoolDown = Time.time + 10;
                    _comboActive = true;
                    _weaponCoolDown = Time.time + action.coolDown;
                    return true;
                }
            }
            else
            {
                if (action.projectilePreFab == null) return false;

                GameObject projectile = Instantiate(action.projectilePreFab, transform.position + (Vector3)(action.attackPoint * movementController.direction), Quaternion.identity);
                projectile.GetComponent<IProjectile>().Init(action.attackPoint * movementController.direction);
            }
            
            return false;
        }

        private IDamageable[] GetHitList(WeaponAction action)
        {
            List<IDamageable> hitList = new List<IDamageable>();

            Collider2D[] entityHitList = Physics2D.OverlapCircleAll(
                (Vector2) transform.position + action.attackPoint * movementController.direction, action.range);

            foreach (Collider2D entity in entityHitList)
            {
                hitList.Add(entity.GetComponent<IDamageable>());
            }

            return hitList.ToArray();
        }
    }
}