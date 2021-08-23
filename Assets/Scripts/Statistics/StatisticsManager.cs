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

        private SceneManager _sceneManager;
        
        public Dictionary<string, object> Dictionary { get; private set; }

        private void Awake() => Instance = this;

        private void OnEnable()
        {
            _sceneManager = SceneManager.Instance;

            _sceneManager.OnEndSceneChange += index =>
            {
                switch (index)
                {
                    case SceneIndexes.Hub:
                    case SceneIndexes.Level:
                    case SceneIndexes.Level0:
                        Save save = SaveManager.Instance.GetCurrentSave;
                        Dictionary = save != null ? save.Statistics : new Dictionary<string, object>();
                        break;
                }
            };

            _sceneManager.OnBeginSceneChange += index =>
            {
                switch (index)
                {
                    case SceneIndexes.Hub:
                    case SceneIndexes.Level:
                    case SceneIndexes.Level0:
                        Save save = SaveManager.Instance.GetCurrentSave;
                        if (save != null) save.Statistics = Dictionary;
                        break;
                }
            };
        }
        
        private void AddKey(string keyPath, object value = null)
        {
            string key = ConvertPath(keyPath, out string[] path);

            Dictionary<string, object> dict = MarchGet(path, Dictionary);
            dict.Add(key, value);
        }

        public void AddFloatValue(string keyPath, float value)
        {
            if (KeyExists(keyPath))
            {
                switch (GetKeyValue(keyPath))
                {
                    case float f:
                        SetKeyValue(keyPath, f + value);
                        break;
                    case string stringy:
                        if (float.TryParse(stringy, out float f2))
                            SetKeyValue(keyPath, f2 + value);
                        break;
                }
            }
            else
                SetKeyValue(keyPath, value);
        }

        public void AddIntValue(string keyPath, int value)
        {
            if (KeyExists(keyPath))
            {
                switch (GetKeyValue(keyPath))
                {
                    case int i:
                        SetKeyValue(keyPath, i + value);
                        break;
                    case string stringy:
                        if (int.TryParse(stringy, out int i2))
                            SetKeyValue(keyPath, i2 + value);
                        break;
                }
            }
            else
                SetKeyValue(keyPath, value);
        }
        
        public void SetKeyValue(string keyPath, object value)
        {
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
            if(!KeyExists(keyPath)) return;

            string key = ConvertPath(keyPath, out string[] path);
            MarchGet(path, Dictionary).Remove(key);
        }
        
        public object GetKeyValue(string keyPath)
        {
            if(!KeyExists(keyPath)) return null;

            string key = ConvertPath(keyPath, out string[] path);
            if(MarchGet(path, Dictionary).TryGetValue(key, out object value)) return value;

            return null;
        }
        
        public bool KeyExists(string keyPath)
        {
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

                string[] newPath = path.Skip(1).ToArray();

                if (newPath.Length <= 0) return objDict;

                path = newPath;
                dict = objDict;
            }
        }
        
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

                string[] newPath = path.Skip(1).ToArray();

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