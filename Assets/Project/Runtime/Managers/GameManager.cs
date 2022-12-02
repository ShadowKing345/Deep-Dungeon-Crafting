using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Project.Runtime.Managers
{
    using Board;
    using Entity.Combat;
    using Entity.Player;
    using Utils;

    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(SceneManager))]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [field: Header("Manager Instances")]
        [field: SerializeField]
        public InputManager InputManager { get; private set; }
        
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
        }
        
        public void LoadLevel(FloorSettings scheme)
        {
            floorSettings = scheme;
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}