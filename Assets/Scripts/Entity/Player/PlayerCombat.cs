using System.Linq;
using Combat;
using Managers;
using UnityEngine;
using Utils;

namespace Entity.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Class")] [SerializeField] private WeaponClass currentWeaponClass;

        [Space] private float actionCoolDown;

        public Player player;
        private PlayerInventory playerInventory;
        private PlayerEntity playerEntity;
        private PlayerMovement playerMovement;

        public delegate void OnWeaponClassChange(WeaponClass weaponClass);
        public OnWeaponClassChange onChangeWeaponClass;

        private void Start()
        {
            playerEntity = player.playerEntity;
            playerMovement = player.playerMovement;
            playerInventory = player.playerInventory;
            playerInventory.WeaponInventory.OnWeaponChanged += ChangeClass;

            CurrentWeaponClass = null;
        }

        private void OnDestroy()
        {
            playerInventory.WeaponInventory.OnWeaponChanged -= ChangeClass;
        }

        public void Attack(WeaponClass.AbilityIndex index)
        {
            Debug.Log(index);

            var ability = currentWeaponClass.GetAbility(index).FirstOrDefault();

            if (!ability) return;
            
            playerMovement.PlayAttackAnimation(ability.AnimationName);
        }

        private void ChangeClass(WeaponClass weaponClass)
        {
            currentWeaponClass = weaponClass;
        }

        public WeaponClass CurrentWeaponClass
        {
            get => currentWeaponClass;
            private set
            {
                currentWeaponClass = value == null ? GameManager.Instance.noWeaponClass : value;
                onChangeWeaponClass?.Invoke(currentWeaponClass);
            }
        }

        [Space] [Header("Attack Point Gizmos Helper")]
        public float abilityRange;

        public float attackOffset;
        public Vector2 centerOffset;
        public Direction direction;
        
        private void OnDrawGizmosSelected()
        {
            if (playerEntity == null) return;
            if (abilityRange <= 0) return;

            Gizmos.DrawWireSphere(
                (Vector2)transform.position + playerEntity.Stats.GetCenterPos + centerOffset +
                direction.AsVector() * attackOffset, abilityRange);
        }
    }
}