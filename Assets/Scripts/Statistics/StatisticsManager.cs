using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
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
            set
            {
                if (_instance != null && _instance != value) Destroy(_instance);
                DontDestroyOnLoad(value);
                _instance = value;
            }
        }

        private static string _fileLocation;
        
        private Dictionary<string, object> _dictionary;

        public Dictionary<string, object> Dictionary => _dictionary;
        
        private void OnEnable()
        {
            Instance = this;
            _fileLocation ??= Path.Combine(Application.dataPath, "stats.stats");

            _dictionary = SaveSystem.TryLoadObj(_fileLocation, out Dictionary<string, object> dictionary)
                ? dictionary
                : new Dictionary<string, object>();
        }

        private void OnDisable() => SaveSystem.SaveObj(_fileLocation, _dictionary);

        private void AddKey(string keyPath, object value = null)
        {
            string key = ConvertPath(keyPath, out string[] path);

            Dictionary<string, object> dict = MarchGet(path, _dictionary);
            dict.Add(key, value);
        }

        public void SetKeyValue(string keyPath, object value)
        {
            if (KeyExists(keyPath))
            {
                string key = ConvertPath(keyPath, out string[] path);

                MarchGet(path, _dictionary)[key] = value;
            }
            else
                AddKey(keyPath, value);
        }

        public void RemoveKey(string keyPath)
        {
            if(!KeyExists(keyPath)) return;

            string key = ConvertPath(keyPath, out string[] path);
            MarchGet(path, _dictionary).Remove(key);
        }

        public object GetKeyValue(string keyPath)
        {
            if(!KeyExists(keyPath)) return null;

            string key = ConvertPath(keyPath, out string[] path);
            if(MarchGet(path, _dictionary).TryGetValue(key, out object value)) return value;

            return null;
        }

        private bool KeyExists(string keyPath)
        {
            try
            {
                string key = ConvertPath(keyPath, out string[] path);
                
                bool flag = MarchCheck(path, _dictionary);

                if (flag)
                    return MarchGet(path, _dictionary).ContainsKey(key);
                
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