using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Project.Runtime.Managers
{
    using Board;
    using Entity.Combat;
    using Entity.Player;
    using Enums;
    using Utils;

    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(SceneManager))]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [field: Header("Manager Instances")]
        [field: SerializeField]
        public InputManager InputManager { get; private set; }

        [field: SerializeField] public SceneManager SceneManager { get; private set; }

        [Header("Game State Data")] public string savePath;
        [SerializeField] private int currentFloor;
        [SerializeField] private FloorSettings floorSettings;
        [SerializeField] private FloorCollection floorCollection;

        public WeaponClass noWeaponClass;
        public ControllerAsset controllerAsset;


        public int CurrentFloor
        {
            get => currentFloor;
            set => currentFloor = value;
        }

        public FloorSettings FloorScheme
        {
            get => floorSettings;
            set => floorSettings = value;
        }

        public static bool PlayerMovement
        {
            set
            {
                var movement = FindObjectOfType<PlayerMovement>();
                if (movement != null) movement.enabled = value;

                var combat = FindObjectOfType<PlayerCombat>();
                if (combat != null) combat.enabled = value;
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                if (Instance == this) return;
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            InputManager = GetComponent<InputManager>();
            SceneManager = GetComponent<SceneManager>();
        }

        private void OnEnable()
        {
            // _sceneManager.OnBeginSceneChange += OnBeginSceneChange;
            // _sceneManager.OnEndSceneChange += OnEndSceneChange;
        }

        private void OnDisable()
        {
            // _sceneManager.OnBeginSceneChange -= OnBeginSceneChange;
            // _sceneManager.OnEndSceneChange -= OnEndSceneChange;
        }

        public event Action OnApplicationClose;

        public void StartLevel()
        {
            // boardManager.CurrentFloor = CurrentFloor;
            // boardManager.FloorSettings = floorSettings;

            StartCoroutine(GenRoom());
        }

        public void NextFloor()
        {
            if (++CurrentFloor >= 10)
            {
                FinishRun();
                return;
            }

            // UiManager.Instance.HudElements.floorNumber.text = $"Floor {floorSettings.StartingFloorNumber + currentFloor}";

            StartCoroutine(GenRoom());
        }

        private IEnumerator GenRoom()
        {
            // LoadingScreenManager.HideScreen();
            // yield return new WaitForEndOfFrame();
            // boardManager.ResetLists();
            // boardManager.InitializeVariables();
            // boardManager.GenerateLayout();
            // boardManager.CreateRooms();
            // boardManager.ConnectRooms();
            // boardManager.FillInRoof();
            // yield return new WaitForFixedUpdate();
            // boardManager.Scan();
            // boardManager.PlacePlayer();
            // yield return new WaitForFixedUpdate();
            // yield return new WaitForEndOfFrame();
            // LoadingScreenManager.ShowScreen();
            yield return null;
        }

        public void LoadLevel(FloorSettings scheme)
        {
            floorSettings = scheme;

            SceneManager.ChangeScene(SceneIndexes.Level, () =>
            {
                currentFloor = 0;
                // boardManager = BoardManager.Instance;
                // boardManager.FloorSettings = floorSettings;
                StartLevel();
            });
        }

        public void LoadHub()
        {
            SceneManager.ChangeScene(SceneIndexes.Hub);
        }

        public void FinishRun()
        {
            // StatisticsManager.Instance.AddIntValue($"Floors.{floorSettings.name}.Cleared", 1);
            // StatisticsManager.Instance.AddIntValue($"Floors.{floorSettings.name}.Total", 1);
            var saveManager = SaveManager.Instance;

            if (saveManager != null)
            {
                var save = saveManager.GetCurrentSave;

                var item = Enumerable.FirstOrDefault(floorCollection.Items,
                    floorItem => floorItem.settings == FloorScheme);
                if (!save.CompletedLevels.Contains(item.levelIndex)) save.CompletedLevels.Add(item.levelIndex);
            }

            UiManager.Instance.ShowUiElement(WindowReference.CompleteScreen);
        }

        public void EndRun()
        {
            // StatisticsManager.Instance.AddIntValue($"Floors.{floorSettings.name}.Failed", 1);
            // StatisticsManager.Instance.AddIntValue($"Floors.{floorSettings.name}.Total", 1);
            FindObjectOfType<PlayerInventory>().SaveInventory = false;
            SceneManager.ChangeScene(SceneIndexes.Hub);
        }

        public void QuitGame()
        {
            if (SceneManager.CurrentScene == SceneIndexes.Level)
                FindObjectOfType<PlayerInventory>().SaveInventory = false;

            SceneManager.ChangeScene(SceneIndexes.MainMenu);
        }

        public void ExitGame()
        {
            OnApplicationClose?.Invoke();
            Application.Quit();
        }

        private void OnBeginSceneChange(SceneIndexes index)
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
                    // PlayerEntity.OnPlayerDeath -= PlayerDeath;
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
                    // UiManager.Instance.HudElements.floorNumber.text = "Hub";
                    break;
                case SceneIndexes.Level:
                case SceneIndexes.Level0:
                    // PlayerEntity.OnPlayerDeath += PlayerDeath;
                    break;
            }
        }

        public void NewGame()
        {
            SceneManager.ChangeScene(SceneIndexes.Level0);
        }
    }
}