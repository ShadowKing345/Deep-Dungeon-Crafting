using System.Collections.Generic;
using System.Linq;
using Board;
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
        
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private GameObject levelSelectEntry;
        [SerializeField] private Transform content;

        private List<FloorScheme> floors;
        private readonly List<Toggle> toggles = new List<Toggle>();

        private void OnEnable()
        {
            _gameManager = GameManager.Instance;
            
            GameObjectUtils.ClearChildren(content);
            floors = Resources.LoadAll<FloorScheme>("Floors").ToList();
            levelSelectEntry.SetActive(false);
            toggles.Clear();
            
            SetupToggles();
        }

        private void SetupToggles()
        {
            foreach (FloorScheme floor in floors)
            {
                GameObject entry = Instantiate(levelSelectEntry, content);
                entry.SetActive(true);
                if (entry.TryGetComponent(out Toggle toggle))
                {
                    toggle.group = toggleGroup;
                    toggles.Add(toggle);
                }

                entry.GetComponentInChildren<TextMeshProUGUI>().text = floor.name;
            }

            if (_gameManager.FloorScheme == null)
                toggles.First().Select();
            else
                toggles[floors.IndexOf(_gameManager.FloorScheme)].Select();
        }

        public void OnStartClick()
        {
            _gameManager.FloorScheme = floors[toggles.IndexOf(toggleGroup.GetFirstActiveToggle())];
            _gameManager.ChangeScene(GameManager.Scenes.Level);
        }
        
        public void Show()
        {
            
        }

        public void Hide()
        {
            
        }
    }
}