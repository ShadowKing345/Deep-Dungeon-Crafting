using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapons;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public WeaponClass noWeaponClass;

        private void OnEnable()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ExitGame()
        {
            Debug.Log("Exit Game");
            Application.Quit();
        }

        public void QuitGame()
        {
            Debug.Log("Quitting Game.");
        }
    }
}