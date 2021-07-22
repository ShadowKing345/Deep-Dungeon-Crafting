using System;
using Board;
using Combat;
using Entity.Player;
using Interfaces;
using UnityEngine;
using Utils;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                _instance ??= FindObjectOfType<GameManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value);
                    return;
                }
                _instance = value;
                DontDestroyOnLoad(value);
            }
        }
        
        private SceneManager _sceneManager;

        public string savePath;

        private BoardManager boardManager;
        [SerializeField] private int currentFloor = 0;
        [SerializeField] private FloorSettings floorSettings;

        public int CurrentFloor { get => currentFloor; set => currentFloor = value; }
        public FloorSettings FloorScheme { get => floorSettings; set => floorSettings = value; }
        
        public WeaponClass noWeaponClass;
        public ControllerAsset controllerAsset;

        public event Action OnApplicationClose;
        
#if UNITY_EDITOR
        [SerializeField] private bool loadMainMenu;
#endif

        private void Awake()
        {
            Instance = this;
            _sceneManager = SceneManager.Instance;

#if UNITY_EDITOR
            if(loadMainMenu) _sceneManager.ChangeScene(SceneIndexes.MainMenu);
#else
            _sceneManager.ChangeScene(SceneIndexes.MainMenu);
#endif
        }

        private void OnEnable()
        {
            _sceneManager.OnBeginSceneChange += OnBeginSceneChange;
            _sceneManager.OnEndSceneChange += OnEndSceneChange;
        }

        private void OnDisable()
        {
            _sceneManager.OnBeginSceneChange -= OnBeginSceneChange;
            _sceneManager.OnEndSceneChange -= OnEndSceneChange;
        }

        public void StartLevel()
        {
            boardManager.CurrentFloor = CurrentFloor;
            boardManager.FloorSettings = floorSettings;
            
            boardManager.ResetLists();
            boardManager.InitializeVariables();
            boardManager.GenerateLayout();
            boardManager.CreateRooms();
            boardManager.ConnectRooms();
            boardManager.FillInRoof();
            boardManager.PlacePlayer();
        }

        public void NextFloor()
        {
            if (++CurrentFloor > 10) { FinishRun(); }
            
            boardManager.ResetLists();
            boardManager.InitializeVariables();
            boardManager.GenerateLayout();
            boardManager.CreateRooms();
            boardManager.ConnectRooms();
            boardManager.FillInRoof();
            boardManager.PlacePlayer();
        }

        public void LoadLevel(FloorSettings scheme)
        {
            floorSettings = scheme;
            
            _sceneManager.ChangeScene(SceneIndexes.Level, () =>
            {
                boardManager = BoardManager.Instance;
                boardManager.FloorSettings = floorSettings;
                StartLevel();
            });
        }

        public void LoadHub(string savePath)
        {
            this.savePath = savePath;

            _sceneManager.ChangeScene(SceneIndexes.Hub);
        }

        public void FinishRun()
        {
            _sceneManager.ChangeScene(SceneIndexes.Hub);
        }
        
        public void EndRun()
        {
            FindObjectOfType<PlayerInventory>().SaveInventory = false;
            _sceneManager.ChangeScene(SceneIndexes.Hub);
        }

        public void QuitGame() => _sceneManager.ChangeScene(SceneIndexes.MainMenu);
        public void ExitGame()
        {
            OnApplicationClose?.Invoke();
            Application.Quit();
        }

        private void OnBeginSceneChange(SceneIndexes index) {
            switch (index)
            {
                case SceneIndexes.Persistent:
                case SceneIndexes.MainMenu:
                case SceneIndexes.Dev:
                    break;
                case SceneIndexes.Hub:
                case SceneIndexes.Level:
                case SceneIndexes.Level0:
                    Player.OnPlayerDeath -= PlayerDeath;
                    break;
            }
        }

        private void OnEndSceneChange(SceneIndexes index)
        {
            switch (index)
            {
                case SceneIndexes.Persistent:
                case SceneIndexes.MainMenu:
                case SceneIndexes.Dev:
                    break;
                case SceneIndexes.Hub:
                case SceneIndexes.Level:
                case SceneIndexes.Level0:
                    Player.OnPlayerDeath += PlayerDeath;
                    break;
            }
        }

        private void PlayerDeath()
        {
            FindObjectOfType<PlayerInventory>().ResetEveryInventory();
            _sceneManager.ChangeScene(SceneIndexes.Hub);
        }
    }
}