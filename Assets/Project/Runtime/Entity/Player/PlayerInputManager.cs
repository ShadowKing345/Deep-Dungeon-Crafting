using System;
using Inputs;
using Project.Runtime.Entity.Combat;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Runtime.Entity.Player
{
    public class PlayerInputManager : MonoBehaviour, InputManager.IPlayerActions
    {
        public PlayerEntity player;

        public float aoeSize;
        public Vector2 aoeOffset;

        private InputManager inputManager;

        private void OnEnable()
        {
            if (inputManager == null)
            {
                inputManager = new InputManager();
                inputManager.Player.SetCallbacks(this);
            }

            inputManager.Player.Enable();
        }

        private void OnDisable()
        {
            inputManager.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (player.playerMovement != null) player.playerMovement.Move(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnAbility1(InputAction.CallbackContext context)
        {
            if (player.playerCombat != null) player.playerCombat.UseAbility(WeaponClass.AbilityIndex.Abilities1);
        }

        public void OnAbility2(InputAction.CallbackContext context)
        {
            if (player.playerCombat != null) player.playerCombat.UseAbility(WeaponClass.AbilityIndex.Abilities2);
        }

        public void OnAbility3(InputAction.CallbackContext context)
        {
            if (player.playerCombat != null) player.playerCombat.UseAbility(WeaponClass.AbilityIndex.Abilities3);
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