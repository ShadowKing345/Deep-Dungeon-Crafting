using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Utils;

namespace Managers
{
    [Serializable]
    public class Save
    {
        [SerializeField] private string name;
        
        private Dictionary<string, object> statistics = new Dictionary<string, object>();
        [SerializeField] private string chestInventory = "";
        [SerializeField] private string playerItemInventory = "";
        [SerializeField] private string playerWeaponInventory = "";
        [SerializeField] private string playerArmorInventory = "";
        [SerializeField] private List<int> completedLevels = new List<int>();

        public string Name
        {
            get => name;
            set => name = value;
        }

        public Dictionary<string, object> Statistics
        {
            get => statistics;
            set => statistics = value;
        }
        
        public string ChestInventory
        {
            get => chestInventory;
            set => chestInventory = value;
        }

        public string PlayerItemInventory
        {
            get => playerItemInventory;
            set => playerItemInventory = value;
        }

        public string PlayerWeaponInventory
        {
            get => playerWeaponInventory;
            set => playerWeaponInventory = value;
        }

        public string PlayerArmorInventory
        {
            get => playerArmorInventory;
            set => playerArmorInventory = value;
        }

        public List<int> CompletedLevels
        {
            get => completedLevels;
            set => completedLevels = value;
        }
    }

    public class SaveManager : MonoBehaviour
    {
        private static SaveManager _instance;
        public static SaveManager Instance
        {
            get
            {
                _instance ??= FindObjectOfType<SaveManager>();
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
            }
        }

        [SerializeField] private Save[] saves = new Save[3];
        private Save currentSave;
#if UNITY_EDITOR
        public int currentSaveIndex;
#endif

        public Save[] Saves => saves;
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            Load();
            GameManager.Instance.OnApplicationClose += Save;
        }

        private void OnDisable() => Save();

        public void Save()
        {
            SaveSystem.SaveObj(Path.Combine(Application.persistentDataPath, "saves.dat"),saves);
        }

        public void Load()
        {
            if (!SaveSystem.TryLoadObj(Path.Combine(Application.persistentDataPath, "saves.dat"), out saves))
                saves = new Save[3];
        }

        public void NewSave(int index)
        {
            if (0 > index || index >= saves.Length) return;

            currentSave = saves[index] = new Save {Name = $"Save {index + 1}"};
        }

        public void LoadSave(int index)
        {
            if (0 > index || index >= saves.Length) return;

            currentSave = saves[index];
#if UNITY_EDITOR
            currentSaveIndex = index;
#endif
        }

        public Save GetCurrentSave => currentSave;
    }
}