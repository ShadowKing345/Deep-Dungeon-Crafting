using System;
using System.Linq;
using Combat;
using Ui.Journal;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Editor
{
    public static class AssetHandler
    {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceId, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceId);
            switch (obj)
            {
                case TabCollection tabCollection:
                    JournalEntryEditorWindow.Open(tabCollection);
                    return true;
                case WeaponClass weaponClass:
                    WeaponClassEntityAiEditor.Open(weaponClass);
                    return true;
            }
            return false;
        }
        
        [MenuItem("Tools/Journal Editor Tool")]
        public static void OpenJournalEditorTool()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(Tab)}");
            JournalEntryEditorWindow.Open(guids.Select(guid => AssetDatabase.LoadAssetAtPath<Tab>(AssetDatabase.GUIDToAssetPath(guid))).Where(tab => tab != null).ToArray());
        }
    }
}