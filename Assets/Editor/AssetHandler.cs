using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Weapons;

namespace Editor
{
    public static class AssetHandler
    {
        public delegate void Open(object obj);
        private static readonly Dictionary<Type, Open> Dictionary = new Dictionary<Type, Open>();

        [OnOpenAsset]
        public static bool OpenEditor(int instanceId, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId);

            if (obj == null) return false;
            if (!Dictionary.TryGetValue(obj.GetType(), out var callback)) return false;
            callback(obj);
            return true;

        }

        public static Open Subscribe(Type type, Open callback)
        {
            if (!Dictionary.ContainsKey(type))
            {
                Dictionary.Add(type, callback);
                return callback;
            }
            return null;
        }
        
        public static bool Unsubscribe(Type type)
        {
            return Dictionary.Remove(type);
        }
    }
}