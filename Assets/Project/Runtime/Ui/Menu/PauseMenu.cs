using Project.Runtime.Entity.Player;
using Project.Runtime.Enums;
using Project.Runtime.Managers;
using Project.Runtime.Utils.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.Ui.Menu
{
    public class PauseMenu : MonoBehaviour, IUiWindow
    {
        private GameManager _gameManager;
        private UiManager _uiManager;
        private PlayerCombat playerCombat;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            _gameManager ??= GameManager.Instance;
            _uiManager ??= UiManager.Instance;

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            playerMovement ??= FindObjectOfType<PlayerMovement>();
            playerCombat ??= FindObjectOfType<PlayerCombat>();
        }

        private void OnDestroy()
        {
        }

        public void Show()
        {
            if (playerCombat != null && playerMovement != null) playerCombat.enabled = playerMovement.enabled = false;
            Time.timeScale = 0;
            GetComponentInChildren<Selectable>().Select();
        }

        public void Hide()
        {
            if (playerCombat != null && playerMovement != null) playerCombat.enabled = playerMovement.enabled = true;
            Time.timeScale = 1;
        }

        public void Resume()
        {
        }

        public void OpenHelp()
        {
            Resume();
        }
    }
}