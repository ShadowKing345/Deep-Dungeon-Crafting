using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Project.Runtime.Utils
{
    public static class SaveSystem
    {
        public static void SaveFileAsBits()
        {
        }

        public static void SaveObj(string filePath, object obj)
        {
            if (obj == null) return;

            var formatter = new BinaryFormatter();
            var stream = new FileStream(filePath, FileMode.Create);

            formatter.Serialize(stream, obj);
            stream.Close();
        }

        public static bool TryLoadObj<T>(string filePath, out T obj) where T : class
        {
            if (File.Exists(filePath))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(filePath, FileMode.Open);

                obj = formatter.Deserialize(stream) as T;
                stream.Close();

                return true;
            }

            // DebugManager.LogWarning($"Cannot find file at {filePath}");
            obj = null;
            return false;
        }
    }
}