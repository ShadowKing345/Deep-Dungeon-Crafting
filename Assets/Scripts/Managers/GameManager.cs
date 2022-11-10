using System;
using System.Collections;
using Board;
using Combat;
using Entity.Player;
using Enums;
using Statistics;
using UnityEngine;
using Utils;
using Enumerable = System.Linq.Enumerable;

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
        [SerializeField] private FloorCollection floorCollection;

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

            StartCoroutine(GenRoom());
        }

        public void NextFloor()
        {
            if (++CurrentFloor >= 10)
            {
                FinishRun();
                return;
            }

            UiManager.Instance.HudElements.floorNumber.text = $"Floor {floorSettings.StartingFloorNumber + currentFloor}";

            StartCoroutine(GenRoom());
        }
        
        private IEnumerator GenRoom()
        {
            LoadingScreenManager.HideScreen();
            yield return new WaitForEndOfFrame();
            boardManager.ResetLists();
            boardManager.InitializeVariables();
            boardManager.GenerateLayout();
            boardManager.CreateRooms();
            boardManager.ConnectRooms();
            boardManager.FillInRoof();
            yield return new WaitForFixedUpdate();
            boardManager.Scan();
            boardManager.PlacePlayer();
            yield return new WaitForFixedUpdate();
            yield return new WaitForEndOfFrame();
            LoadingScreenManager.ShowScreen();
        }

        public void LoadLevel(FloorSettings scheme)
        {
            floorSettings = scheme;
            
            _sceneManager.ChangeScene(SceneIndexes.Level, () =>
            {
                currentFloor = 0;
                boardManager = BoardManager.Instance;
                boardManager.FloorSettings = floorSettings;
                StartLevel();
            });
        }

        public void LoadHub() => _sceneManager.ChangeScene(SceneIndexes.Hub);

        public void FinishRun()
        {
            StatisticsManager.Instance.AddIntValue($"Floors.{floorSettings.name}.Cleared", 1);
            StatisticsManager.Instance.AddIntValue($"Floors.{floorSettings.name}.Total", 1);
            SaveManager saveManager = SaveManager.Instance;

            if (saveManager != null)
            {
                Save save = saveManager.GetCurrentSave;

                FloorCollection.FloorItem item = Enumerable.FirstOrDefault(floorCollection.Items, floorItem => floorItem.settings == FloorScheme);
                if (!save.CompletedLevels.Contains(item.levelIndex)) save.CompletedLevels.Add(item.levelIndex);
            }
            
            UiManager.Instance.ShowUiElement(WindowReference.CompleteScreen);
        }

        public void EndRun()
        {
            StatisticsManager.Instance.AddIntValue($"Floors.{floorSettings.name}.Failed", 1);
            StatisticsManager.Instance.AddIntValue($"Floors.{floorSettings.name}.Total", 1);
            FindObjectOfType<PlayerInventory>().SaveInventory = false;
            _sceneManager.ChangeScene(SceneIndexes.Hub);
        }

        public void QuitGame()
        {
            if (_sceneManager.CurrentScene == SceneIndexes.Level)
                FindObjectOfType<PlayerInventory>().SaveInventory = false;
            
            _sceneManager.ChangeScene(SceneIndexes.MainMenu);
        }

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
                    PlayerEntity.OnPlayerDeath -= PlayerDeath;
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
                    UiManager.Instance.HudElements.floorNumber.text = "Hub";
                    break;
                case SceneIndexes.Level:
                case SceneIndexes.Level0:
                    PlayerEntity.OnPlayerDeath += PlayerDeath;
                    break;
            }
        }

        public void NewGame() => _sceneManager.ChangeScene(SceneIndexes.Level0);

        private void PlayerDeath() => UiManager.Instance.ShowUiElement(WindowReference.DeathScreen);

        public static bool PlayerMovement
        {
            set
            {
                PlayerMovementManager movementManager = FindObjectOfType<PlayerMovementManager>();
                if (movementManager != null) movementManager.enabled = value;

                PlayerCombat combat = FindObjectOfType<PlayerCombat>();
                if (combat != null) combat.enabled = value;
            }
        }
    }
}