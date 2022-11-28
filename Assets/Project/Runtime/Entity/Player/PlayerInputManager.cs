using System;
using Project.Runtime.Entity.Combat;
using Project.Runtime.Managers;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using InputManager = Inputs.InputManager;

namespace Project.Runtime.Entity.Player
{
    public class PlayerInputManager : MonoBehaviour, InputManager.IPlayerActions
    {
        [field: SerializeField] public PlayerEntity Entity { get; set; }

        [Space] public float aoeSize;
        public Vector2 aoeOffset;

        private InputManager manager;

        private void Start()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null || gameManager.InputManager == null)
            {
                return;
            }

            manager = gameManager.InputManager.Manager;
            manager.Player.SetCallbacks(this);
            manager.Player.Enable();
        }

        private void OnEnable() => manager?.Player.Enable();
        private void OnDisable() => manager?.Player.Disable();

        public void OnMove(InputAction.CallbackContext context)
        {
            if (Entity.Movement != null) Entity.Movement.Move(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnAbility1(InputAction.CallbackContext context)
        {
            if (Entity.Combat != null) Entity.Combat.UseAbility(WeaponClass.AbilityIndex.Abilities1);
        }

        public void OnAbility2(InputAction.CallbackContext context)
        {
            if (Entity.Combat != null) Entity.Combat.UseAbility(WeaponClass.AbilityIndex.Abilities2);
        }

        public void OnAbility3(InputAction.CallbackContext context)
        {
            if (Entity.Combat != null) Entity.Combat.UseAbility(WeaponClass.AbilityIndex.Abilities3);
        }

        private void SelectInteractable()
        {
            var currentPos = (Vector2) transform.position + aoeOffset;
            var hits = new Collider2D[] { };
            Physics2D.OverlapCircleNonAlloc(currentPos, aoeSize, hits);

            IInteractable closest = null;
            var smallestPos = Vector2.positiveInfinity;

            foreach (var hit in hits)
            {
                if (hit.gameObject == gameObject) continue;
                if (!hit.TryGetComponent(out IInteractable interactable)) continue;

                if (Vector2.Distance(currentPos, hit.transform.position) >=
                    Vector2.Distance(currentPos, smallestPos)) continue;

                smallestPos = hit.transform.position;
                closest = interactable;
            }

            closest?.Interact(gameObject);
        }
    }
}