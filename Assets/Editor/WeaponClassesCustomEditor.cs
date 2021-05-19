using Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Editor
{
    public class AssetHandler
    {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceId, int line)
        {
            WeaponClasses obj = EditorUtility.InstanceIDToObject(instanceId) as WeaponClasses;
    
            if (obj != null)
            {
                WeaponClassesEditorWindow.Open(obj);
                
                return true;
            }
    
            return false;
        }
    }
    
    [CustomEditor(typeof(WeaponClasses))]
    public class WeaponClassesCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                WeaponClassesEditorWindow.Open((WeaponClasses) target);
            }
            base.OnInspectorGUI();
        }
    }
}