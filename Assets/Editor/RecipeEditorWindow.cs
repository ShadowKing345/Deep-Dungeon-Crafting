using System;
using Crafting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class RecipeEditorWindow : EditorWindow
    {
        private SoCollection<Recipe> _collection;
        private bool IsCollectionNull => _collection == null;
        private SerializedObject _recipe;
        private Recipe _selectedRecipe;
        private bool IsRecipeNull => _recipe == null;

        private Vector2 scrollPos;

        public static void Open(Recipe recipe)
        {
            RecipeEditorWindow window = GetWindow<RecipeEditorWindow>("Recipe Editor Window");
            window._recipe = new SerializedObject(recipe);
        }

        public static void Open(Recipe[] recipes)
        {
            RecipeEditorWindow window = GetWindow<RecipeEditorWindow>("Recipe Editor Window");
            window._collection = new SoCollection<Recipe> {data = recipes};
        }

        private void OnGUI()
        {

            if (!IsRecipeNull)
            { 
                DisplayRecipe(_recipe);
                return;
            }

            if (IsCollectionNull) return;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(120), GUILayout.ExpandHeight(true));
                {
                    foreach (Recipe recipe in _collection.data)
                    {
                        if (GUILayout.Button(recipe.name)) _selectedRecipe = recipe;
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");
                {
                    if (_selectedRecipe == null) EditorGUILayout.LabelField("Please select a recipe from the left.");
                    else DisplayRecipe(new SerializedObject(_selectedRecipe));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayRecipe(SerializedObject recipe)
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.PrefixLabel("Result:");
                DisplayItemStack(recipe.FindProperty("result"));

                EditorGUILayout.PrefixLabel("Ingredients:");
                EditorGUILayout.BeginVertical("box");
                {
                    SerializedProperty current = recipe.FindProperty("ingredients");
                    EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
                    {
                        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                        {
                            foreach (SerializedProperty p in current)
                            {
                                DisplayItemStack(p);
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Add")) current.arraySize++;
                        if (GUILayout.Button("Remove")) current.arraySize--;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            recipe.ApplyModifiedProperties();
        }

        private void DisplayItemStack(SerializedProperty itemStack)
        {
            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.PropertyField(itemStack);
                // EditorGUILayout.BeginVertical();
                // {
                //     EditorGUILayout.PropertyField(itemStack.FindPropertyRelative("item"));
                //     EditorGUILayout.PropertyField(itemStack.FindPropertyRelative("amount"));
                // }
                // EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    [CustomEditor(typeof(Recipe))]
    public class RecipeInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor")) RecipeEditorWindow.Open(target as Recipe);

            base.OnInspectorGUI();
        }
    }
}