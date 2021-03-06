using System.Collections.Generic;
using System.Linq;
using Board;
using Enums;
using Interfaces;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace LevelSelect
{
    public class LevelSelectMenu : MonoBehaviour, IUiWindow
    {
        private GameManager _gameManager;
        private UiManager _uiManager;
        
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private GameObject levelSelectEntry;
        [SerializeField] private Transform content;
        [SerializeField] private FloorCollection floors;
        
        private readonly Dictionary<Toggle, FloorSettings> toggles = new Dictionary<Toggle, FloorSettings>();
        private Save save;
            
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
            GameObjectUtils.ClearChildren(content);
            levelSelectEntry.SetActive(false);
            toggles.Clear();
            
            SaveManager saveManager = SaveManager.Instance;
            if (saveManager == null) return;

            save = saveManager.GetCurrentSave;
            
            SetupToggles();
        }

        private void SetupToggles()
        {
            save ??= new Save();
            
            foreach (FloorCollection.FloorItem item in floors.Items)
            {
                if (!save.CompletedLevels.Contains(item.completedFloorIndex) && item.levelIndex != 0) continue;
            
                GameObject entry = Instantiate(levelSelectEntry, content);
                entry.SetActive(true);
                if (entry.TryGetComponent(out Toggle toggle))
                {
                    toggle.group = toggleGroup;
                    toggles.Add(toggle, item.settings);
                }
                
                entry.GetComponentInChildren<TextMeshProUGUI>().text = item.settings.name;
            }

            if (_gameManager.FloorScheme == null)
                toggles.First().Key.Select();
            else if(toggles.ContainsValue(_gameManager.FloorScheme))
                toggles.First(kvPair => kvPair.Value == _gameManager.FloorScheme).Key.Select();
        }

        public void OnStartClick()
        {
            Toggle toggle = toggles.Keys.FirstOrDefault(t => t.isOn);
            if (toggle == null) return;

            if (!toggles.TryGetValue(toggle, out FloorSettings settings)) return;
            _gameManager.LoadLevel(settings);
            _uiManager.HideUiElement(WindowReference.LevelSelector);
        }
        
        public void Show() => GameManager.PlayerMovement = false;
        public void Hide() => GameManager.PlayerMovement = true;
    }
}