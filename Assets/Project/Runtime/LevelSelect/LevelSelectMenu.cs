using System.Collections.Generic;
using System.Linq;
using Project.Runtime.Board;
using Project.Runtime.Managers;
using Project.Runtime.Utils;
using Project.Runtime.Utils.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Runtime.LevelSelect
{
    public class LevelSelectMenu : MonoBehaviour, IUiWindow
    {
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private GameObject levelSelectEntry;
        [SerializeField] private Transform content;
        [SerializeField] private FloorCollection floors;

        private readonly Dictionary<Toggle, FloorSettings> toggles = new();
        private GameManager _gameManager;
        private UiManager _uiManager;
        private Save save;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _uiManager = UiManager.Instance;

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GameObjectUtils.ClearChildren(content);
            levelSelectEntry.SetActive(false);
            toggles.Clear();

            var saveManager = SaveManager.Instance;
            if (saveManager == null) return;

            save = saveManager.GetCurrentSave;

            SetupToggles();
        }
        
        public void Show()
        {
            GameManager.PlayerMovement = false;
        }

        public void Hide()
        {
            GameManager.PlayerMovement = true;
        }

        private void SetupToggles()
        {
            save ??= new Save();

            foreach (var item in floors.Items)
            {
                if (!save.CompletedLevels.Contains(item.completedFloorIndex) && item.levelIndex != 0) continue;

                var entry = Instantiate(levelSelectEntry, content);
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
            else if (toggles.ContainsValue(_gameManager.FloorScheme))
                toggles.First(kvPair => kvPair.Value == _gameManager.FloorScheme).Key.Select();
        }

        public void OnStartClick()
        {
            var toggle = toggles.Keys.FirstOrDefault(t => t.isOn);
            if (toggle == null) return;

            if (!toggles.TryGetValue(toggle, out var settings)) return;
            _gameManager.LoadLevel(settings);
        }
    }
}