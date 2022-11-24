using UnityEngine;

namespace Project.Runtime.Entity.Player
{
    public class Player : MonoBehaviour
    {
        public PlayerInputManager playerInputManager;
        public PlayerMovement playerMovement;
        public PlayerEntity playerEntity;
        public PlayerInventory playerInventory;
        public PlayerCombat playerCombat;

        private void Awake()
        {
            if (playerInputManager == null)
            {
                playerInputManager = GetComponentInChildren<PlayerInputManager>();
                if (playerInputManager != null) playerInputManager.player = this;
            }

            if (playerMovement == null)
            {
                playerMovement = GetComponentInChildren<PlayerMovement>();
                if (playerMovement != null) playerMovement.player = this;
            }

            if (playerEntity == null)
            {
                playerEntity = GetComponentInChildren<PlayerEntity>();
                if (playerEntity != null) playerEntity.player = this;
            }

            if (playerInventory == null)
            {
                playerInventory = GetComponentInChildren<PlayerInventory>();
                if (playerInventory != null) playerInventory.player = this;
            }

            // ReSharper disable once InvertIf
            if (playerCombat == null)
            {
                playerCombat = GetComponentInChildren<PlayerCombat>();
                if (playerCombat != null) playerCombat.player = this;
            }
        }
    }
}