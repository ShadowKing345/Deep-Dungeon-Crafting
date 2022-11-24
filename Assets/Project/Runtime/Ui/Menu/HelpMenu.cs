using Project.Runtime.Entity.Player;
using Project.Runtime.Enums;
using Project.Runtime.Managers;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;

namespace Project.Runtime.Ui.Menu
{
    public class HelpMenu : MonoBehaviour, IUiWindow
    {
        private UiManager _uiManager;
        private PlayerCombat playerCombat;

        private PlayerMovement playerMovement;

        private void Awake()
        {
            _uiManager = UiManager.Instance;
            _uiManager.RegisterWindow(WindowReference.Help, gameObject);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            playerCombat ??= FindObjectOfType<PlayerCombat>();
            playerMovement ??= FindObjectOfType<PlayerMovement>();
        }

        private void OnDestroy()
        {
            _uiManager.UnregisterWindow(WindowReference.Help, gameObject);
        }

        public void Show()
        {
            playerCombat.enabled = playerMovement.enabled = false;
            Time.timeScale = 0;
        }

        public void Hide()
        {
            playerCombat.enabled = playerMovement.enabled = true;
            Time.timeScale = 1;
        }
    }
}