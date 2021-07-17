using System;
using System.IO;
using Managers;
using TMPro;
using UnityEngine;
using Application = UnityEngine.Application;

namespace Ui.Saves
{
    public class SaveManager : MonoBehaviour
    {
        private GameManager _gameManager;
        
        public enum ControllerState
        {
            NewGame,
            LoadGame
        }

        public ControllerState State { get; set; }

        [SerializeField] private string savesPath;

        private void OnEnable()
        {
            _gameManager = GameManager.Instance;
            
            savesPath = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(savesPath)) Directory.CreateDirectory(savesPath);

            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();

            int i = 1;
            foreach (TextMeshProUGUI text in texts)
            {
                string path = Path.Combine(savesPath, $"Save{i}");
                text.text = Directory.Exists(path) ? $"Save {i++}" : "Empty";
            }
        }

        public void OnButtonClick(int index)
        {
            string path = Path.Combine(savesPath, $"Save{index}");
            _gameManager.savePath = path;
            switch (State)
            {
                case ControllerState.NewGame:
                    New(path);
                    break;
                case ControllerState.LoadGame:
                    Load(path);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Load(string path)
        {
            if (!Directory.Exists(path))
            {
                New(path);
                return;
            }
            
            _gameManager.ChangeScene(GameManager.Scenes.Hub);
        }

        private void New(string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path);

            Directory.CreateDirectory(path);
            
            _gameManager.ChangeScene(GameManager.Scenes.Hub);
        }
    }
}