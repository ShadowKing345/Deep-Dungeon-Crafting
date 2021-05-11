using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Weapons;

namespace Entity.Player
{
    public class PlayerEntity : Entity
    {
        public UnityEvent playerDied;

        private void Start()
        {
            playerDied ??= new UnityEvent();
        }
        
        private void Update()
        {
            if (!(Time.time >= weaponCooldown)) return;
            if (weaponClass == null) return;

            WeaponAction action = null;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                action = weaponClass.action1.FirstOrDefault();
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                action = weaponClass.action2.FirstOrDefault();
            else if (Input.GetKeyDown(KeyCode.Alpha3)) action = weaponClass.action3.FirstOrDefault();

            if (action == null) return;
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Attack(action);
            weaponCooldown = Time.time + action.coolDown;

        }

        public override void Die()
        {
            Debug.Log("Player has died.");
            base.Die();
            playerDied.Invoke();
        }
    }
}