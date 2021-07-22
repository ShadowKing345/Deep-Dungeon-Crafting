using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Managers;
using UnityEngine;
using Utils;

namespace Statistics
{
    public class StatisticsManager : MonoBehaviour
    {
        private static StatisticsManager _instance;
        public static StatisticsManager Instance
        {
            get
            {
                _instance ??= FindObjectOfType<StatisticsManager>();
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

        private GameManager _gameManager;
        private SceneManager _sceneManager;
        
        private static string _fileLocation;

        public Dictionary<string, object> Dictionary { get; private set; }

        private void Awake() => Instance = this;

        private void OnEnable()
        {
            _gameManager = GameManager.Instance;
            _sceneManager = SceneManager.Instance;

            _gameManager.OnApplicationClose += Save;
            _sceneManager.OnBeginSceneChange += OnBeginSceneChange;
            _sceneManager.OnEndSceneChange += OnEndSceneChange;
        }
        
        private void OnDisable()
        {
            _gameManager.OnApplicationClose -= Save;
            _sceneManager.OnBeginSceneChange -= OnBeginSceneChange;
            _sceneManager.OnEndSceneChange -= OnEndSceneChange;
            Save();
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
                    Save();
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
                    _fileLocation = Path.Combine(_gameManager.savePath, "statistics.stats");
                    Load();
                    break;
            }
                
        }
        
        public void Load()
        {
            if (!isReady) return;
            
            if (!string.IsNullOrEmpty(_fileLocation))
                Dictionary = SaveSystem.TryLoadObj(_fileLocation, out Dictionary<string, object> dictionary)
                    ? dictionary
                    : new Dictionary<string, object>();
        }

        public void Save()
        {
            if (!isReady) return;

            if(!string.IsNullOrEmpty(_fileLocation)) SaveSystem.SaveObj(_fileLocation, Dictionary);
        }

        private void AddKey(string keyPath, object value = null)
        {
            if (!isReady) return;

            string key = ConvertPath(keyPath, out string[] path);

            Dictionary<string, object> dict = MarchGet(path, Dictionary);
            dict.Add(key, value);
        }
        
        public void SetKeyValue(string keyPath, object value)
        {
            if (!isReady) return;

            if (KeyExists(keyPath))
            {
                string key = ConvertPath(keyPath, out string[] path);

                MarchGet(path, Dictionary)[key] = value;
            }
            else
                AddKey(keyPath, value);
        }
        
        public void RemoveKey(string keyPath)
        {
            if (!isReady) return;

            if(!KeyExists(keyPath)) return;

            string key = ConvertPath(keyPath, out string[] path);
            MarchGet(path, Dictionary).Remove(key);
        }
        
        public object GetKeyValue(string keyPath)
        {
            if (!isReady) return null;

            if(!KeyExists(keyPath)) return null;

            string key = ConvertPath(keyPath, out string[] path);
            if(MarchGet(path, Dictionary).TryGetValue(key, out object value)) return value;

            return null;
        }
        
        public bool KeyExists(string keyPath)
        {
            if (!isReady) return false;

            try
            {
                string key = ConvertPath(keyPath, out string[] path);
                
                bool flag = MarchCheck(path, Dictionary);

                if (flag)
                    return MarchGet(path, Dictionary).ContainsKey(key);
                
            }
            catch (InvalidDataException) { }
 
            return false;
        }
        
        private static Dictionary<string, object> MarchGet(string[] path, Dictionary<string, object> dict)
        {
            while (true)
            {
                Dictionary<string, object> objDict = new Dictionary<string, object>();
                
                if (dict.TryGetValue(path[0], out var obj))
                {
                    if (!(obj is Dictionary<string, object> dictionary)) throw new InvalidDataException($"The path {string.Join(".", path)} does not contain a dictionary.");
                    objDict = dictionary;
                }
                else
                    dict.Add(path[0], objDict);

                string[] newPath = path.Skip(1).Take(path.Length - 2).ToArray();

                if (newPath.Length <= 0) return objDict;

                path = newPath;
                dict = objDict;
            }
        }

        private bool isReady => File.Exists(_fileLocation);
        
        private static bool MarchCheck(string[] path, Dictionary<string, object> dict)
        {
            if (dict == null) return false;
            
            while (true)
            {
                Dictionary<string, object> objDict;

                if (dict.TryGetValue(path[0], out var obj))
                {
                    if (!(obj is Dictionary<string, object> dictionary))
                        return false;
                    
                    objDict = dictionary;
                }
                else
                    return false;

                string[] newPath = path.Skip(1).Take(path.Length - 2).ToArray();

                if (newPath.Length <= 0) return true;

                path = newPath;
                dict = objDict;
            }
        }

        private static string ConvertPath(string keyPath, out string[] path)
        {
            string[] keys = keyPath.Split('.');
            path = keys.Take(keys.Length -1).ToArray();

            return keys.Last();
        }
    }
}