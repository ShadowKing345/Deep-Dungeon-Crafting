using Ui.ContextMenu;
using UnityEditor;
using UnityEditor.UI;

namespace Editor.Inspectors
{
    [CustomEditor(typeof(ContextMenuEntry))]
    public class ContextMenuEntryInspector : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("text"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("foldOut"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("trigger"));
            EditorGUILayout.Separator();
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}