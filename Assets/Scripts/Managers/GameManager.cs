using System.Collections;
using Board;
using Combat;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
                if(_instance != null && _instance != value) Destroy(value);
                _instance = value;
                DontDestroyOnLoad(value);
            }
        }
        
        public string savePath;

        private BoardManager boardManager;
        [SerializeField] private int currentFloor = -1;
        [SerializeField] private FloorScheme floorScheme;
            
        private int CurrentFloor { get => currentFloor; set => currentFloor = value; }
        public FloorScheme FloorScheme { get => floorScheme; set => floorScheme = value; }
        
        public WeaponClass noWeaponClass;
        public ControllerAsset controllerAsset;
        
        [SerializeField] private Scenes currentScene;
        
        private void OnEnable()
        {
            Instance = this;
            boardManager = BoardManager.Instance;

            if (currentScene != Scenes.Level) return;
            
            currentFloor = -1;
            StartCoroutine(DelayedAction());
        }

        IEnumerator DelayedAction()
        {
            yield return new WaitForFixedUpdate();
            NextFloor();
        }

        public void SetupBoard()
        {
            boardManager.ResetBoard();
            boardManager.InitVariables();
            boardManager.CreateRoomLayout();
            boardManager.CreateRoomObj();
            boardManager.LinkRoomsTogether();
            boardManager.GenerateRoomsTiles();
            boardManager.SetupPathFinder();
            boardManager.GenerateRoomObject();
            boardManager.PlacePlayer();
        }

        public void NextFloor()
        {
            if (boardManager == null) return;
            // blacken the screen.

            if (++CurrentFloor >= 10) { ChangeScene(Scenes.Hub); }
            boardManager.FloorNumber = CurrentFloor;

            // clear the board.
            boardManager.ResetBoard();
            
            // load board with new floor data.
            boardManager.ResetBoard();
            boardManager.InitVariables();
            boardManager.CreateRoomLayout();
            boardManager.CreateRoomObj();
            boardManager.LinkRoomsTogether();
            boardManager.GenerateRoomsTiles();
            boardManager.SetupPathFinder();
            boardManager.GenerateRoomObject();
            boardManager.PlacePlayer();
        }
        
        public void EndRun() => ChangeScene(Scenes.Hub);
        public void ExitGame() => Application.Quit();
        public void QuitGame() => ChangeScene(Scenes.MainMenu);

        public void ChangeScene(Scenes scene)
        {
            currentScene = scene;
            SceneManager.LoadScene((int) scene);
        }

        public enum Scenes
        {
            MainMenu = 0,
            Dev = 1,
            Hub = 2,
            Level = 3,
            Level0 = 4
        }
    }
}