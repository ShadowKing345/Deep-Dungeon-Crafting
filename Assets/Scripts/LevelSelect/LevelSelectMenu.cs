using System.Collections.Generic;
using System.Linq;
using Board;
using Entity.Player;
using Enums;
using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace LevelSelect
{
    public class LevelSelectMenu : MonoBehaviour, IUiWindow
    {
        private GameManager _gameManager;
        private UiManager _uiManager;

        private PlayerMovement playerMovement;
        private PlayerCombat playerCombat;
        
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private GameObject levelSelectEntry;
        [SerializeField] private Transform content;

        private List<FloorSettings> floors;
        private readonly List<Toggle> toggles = new List<Toggle>();

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _uiManager = UiManager.Instance;
            _uiManager.RegisterWindow(WindowReference.LevelSelector, gameObject);
            
            gameObject.SetActive(false);
        }

        private void OnDestroy() => _uiManager.UnregisterWindow(WindowReference.LevelSelector, gameObject);

        private void OnEnable()
        {
            playerCombat ??= FindObjectOfType<PlayerCombat>();
            playerMovement ??= FindObjectOfType<PlayerMovement>();

            GameObjectUtils.ClearChildren(content);
            floors = Resources.LoadAll<FloorSettings>("Floors").ToList();
            levelSelectEntry.SetActive(false);
            toggles.Clear();
            
            SetupToggles();
        }

        private void SetupToggles()
        {
            foreach (FloorSettings floor in floors)
            {
                GameObject entry = Instantiate(levelSelectEntry, content);
                entry.SetActive(true);
                if (entry.TryGetComponent(out Toggle toggle))
                {
                    toggle.group = toggleGroup;
                    toggles.Add(toggle);
                }

                // entry.GetComponentInChildren<TextMeshProUGUI>().text = floor.name;
            }

            if (_gameManager.FloorScheme == null)
                toggles.First().Select();
            else
                toggles[floors.IndexOf(_gameManager.FloorScheme)].Select();
        }

        public void OnStartClick()
        {
            _gameManager.LoadLevel(floors[toggles.IndexOf(toggleGroup.GetFirstActiveToggle())]);
            _uiManager.HideUiElement(WindowReference.LevelSelector);
        }
        
        public void Show() => playerMovement.enabled = playerCombat.enabled = false;
        public void Hide() => playerMovement.enabled = playerCombat.enabled = true;
    }
}