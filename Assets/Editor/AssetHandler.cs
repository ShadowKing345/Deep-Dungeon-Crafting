using System;
using System.Linq;
using Combat;
using Crafting;
using Editor.EditorWindows;
using InGameHelp;
using Ui.Help;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Editor
{
    public static class AssetHandler
    {
        [OnOpenAsset]
        public static bool OpenEditor(int instanceId, int line)
        {
            switch (EditorUtility.InstanceIDToObject(instanceId))
            {
                case TabCollection tabCollection:
                    HelpEntryEditorWindow.Open(tabCollection);
                    return true;
                case WeaponClass weaponClass:
                    WeaponClassEditorWindow.Open(weaponClass);
                    return true;
                case Recipe recipe:
                    RecipeEditorWindow.Open(recipe);
                    return true;
            }

            return false;
        }

        [MenuItem("Tools/Journal Editor Tool")]
        public static void OpenJournalEditorTool() => HelpEntryEditorWindow.Open(AssetDatabase
            .FindAssets($"t:{typeof(Tab)}")
            .Select(guid => AssetDatabase.LoadAssetAtPath<Tab>(AssetDatabase.GUIDToAssetPath(guid))).ToArray());

        [MenuItem("Tools/Recipe Editor Tool")]
        public static void OpenRecipeEditorTool() => RecipeEditorWindow.Open();

        [MenuItem("Tools/Item Editor Tool")]
        public static void OpenItemEditorTool() => ItemEditorWindow.Open();

        [MenuItem("Tools/Weapon Class Editor Tool")]
        public static void OpenWeaponClassEditorTool() => WeaponClassEditorWindow.Open();

    }
}