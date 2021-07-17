using Ui.Crafting;
using UnityEditor;
using UnityEditor.UI;

namespace Editor.Inspectors
{
    [CustomEditor(typeof(RecipeEntry))]
    public class RecipeEntryInspector : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("recipe"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("recipeImage"));

            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing, false);
            
            base.OnInspectorGUI();
        }
    }
}