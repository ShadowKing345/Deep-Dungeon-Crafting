using System;
using UnityEditor;
using UnityEditor.Callbacks;
using Weapons;

namespace Editor
{
    public static class AssetHandler
    {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceId, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId);

            if (obj != null)
            {
                if (obj.GetType() == typeof(WeaponClass))
                {
                    WeaponClassEditorWindow.Open((WeaponClass) obj);
                }
            }

            return false;
        }
    }
}