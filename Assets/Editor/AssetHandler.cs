using System;
using System.Linq;
using Combat;
using Crafting;
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
            switch (EditorUtility.InstanceIDToObject(instanceId))
            {
                case TabCollection tabCollection:
                    JournalEntryEditorWindow.Open(tabCollection);
                    return true;
                case WeaponClass weaponClass:
                    WeaponClassEntityAiEditor.Open(weaponClass);
                    return true;
                case Recipe recipe:
                    RecipeEditorWindow.Open(recipe);
                    return true;
            }

            return false;
        }

        [MenuItem("Tools/Journal Editor Tool")]
        public static void OpenJournalEditorTool() => JournalEntryEditorWindow.Open(AssetDatabase
            .FindAssets($"t:{typeof(Tab)}")
            .Select(guid => AssetDatabase.LoadAssetAtPath<Tab>(AssetDatabase.GUIDToAssetPath(guid))).ToArray());

        [MenuItem("Tools/Recipe Editor Tool")]
        public static void OpenRecipeEditorTool() => RecipeEditorWindow.Open(AssetDatabase
            .FindAssets($"t:{typeof(Recipe)}")
            .Select(guid => AssetDatabase.LoadAssetAtPath<Recipe>(AssetDatabase.GUIDToAssetPath(guid))).ToArray());
    }
}