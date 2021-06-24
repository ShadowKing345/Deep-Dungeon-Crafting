using System;
using Combat;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        public WeaponClass noWeaponClass;

        private void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this);
                instance = this;
            }
            else if (instance != this) Destroy(gameObject);
        }

        public void SetupBoard()
        {
            BoardManager boardManager = BoardManager.instance;
            
            boardManager.ResetLists();
            boardManager.InitVariables();
            boardManager.CreateRoomLayout();
            boardManager.CreateRoomObj();
            boardManager.LinkRoomsTogether();
            boardManager.GenerateRoomsTiles();

            GameObject.Find("Player").transform.position = boardManager.GetPlayerSpawnPoint();
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene((int) Scenes.Dev);
        }

        public void EndRun()
        {
            QuitGame();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void QuitGame()
        {
            SceneManager.LoadScene((int) Scenes.MainMenu);
        }

        private enum Scenes
        {
            MainMenu = 0,
            Dev = 1,
            Hub = 2,
            Level = 3
        }
    }
}