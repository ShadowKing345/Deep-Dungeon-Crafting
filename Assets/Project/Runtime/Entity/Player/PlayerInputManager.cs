using Project.Runtime.Utils.Debug;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Runtime.Entity.Player
{
    using Combat;
    using Managers;
    using Utils.Interfaces;

    public class PlayerInputManager : MonoBehaviour, Inputs.InputManager.IPlayerActions
    {
        [field: SerializeField] public PlayerEntity Entity { get; set; }

        [Space] public float aoeSize;
        public Vector2 aoeOffset;

        private bool isCombatNotNull;
        private PlayerCombat combat;
        private bool isMovementNotNull;
        private PlayerMovement movement;

        private Inputs.InputManager manager;

        private void Start()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            if (Entity.Combat != null)
            {
                isCombatNotNull = true;
                combat = Entity.Combat;
            }

            if (Entity.Movement != null)
            {
                isMovementNotNull = true;
                movement = Entity.Movement;
            }

            if (gameManager.InputManager == null)
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
            if (isMovementNotNull)
            {
                movement.Move(context.ReadValue<Vector2>());
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SelectInteractable();
            }
        }

        public void OnAbility1(InputAction.CallbackContext context)
        {
            if (isCombatNotNull && context.performed)
            {
                combat.UseAbility(WeaponClass.AbilityIndex.Abilities1);
            }
        }

        public void OnAbility2(InputAction.CallbackContext context)
        {
            if (isCombatNotNull && context.performed)
            {
                combat.UseAbility(WeaponClass.AbilityIndex.Abilities2);
            }
        }

        public void OnAbility3(InputAction.CallbackContext context)
        {
            if (isCombatNotNull && context.performed)
            {
                combat.UseAbility(WeaponClass.AbilityIndex.Abilities3);
            }
        }

        public void OnOpenDebugMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DebugController.Instance.OpenDebugMenu();
            }
        }

        private void SelectInteractable()
        {
            var currentPos = (Vector2) transform.position + aoeOffset;

            IInteractable closest = null;
            var smallestPos = Vector2.positiveInfinity;

            // ReSharper disable once Unity.PreferNonAllocApi
            foreach (var hit in Physics2D.OverlapCircleAll(currentPos, aoeSize))
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