using Ui;
using UnityEditor;
using UnityEditor.UI;

namespace Editor.Inspectors
{
    [CustomEditor(typeof(CloseWindowButton))]
    public class CloseWindowButtonInspector : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("uiElement"));
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Separator();
            base.OnInspectorGUI();
        }
    }
}