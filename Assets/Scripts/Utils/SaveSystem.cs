using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Utils
{
    public static class SaveSystem
    {
        public static void SaveFileAsBits()
        {
            
        }
        
        public static void SaveObj(string filePath, object obj)
        {
            if (obj == null) return;
            
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Create);
            
            formatter.Serialize(stream, obj);
            stream.Close();
        }
        
        public static bool TryLoadObj<T>(string filePath, out T obj) where T : class
        {
            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(filePath, FileMode.Open);

                obj = formatter.Deserialize(stream) as T;
                stream.Close();
                
                return true;
            }

            Debug.LogWarning($"Cannot find file at {filePath}");
            obj = null;
            return false;
        }
    }
}