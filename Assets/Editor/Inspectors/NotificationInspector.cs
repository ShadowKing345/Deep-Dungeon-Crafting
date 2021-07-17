using Ui.Notifications;
using UnityEditor;
using UnityEditor.UI;

namespace Editor.Inspectors
{
    [CustomEditor(typeof(Notification))]
    public class NotificationInspector : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("text"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("targetImage"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cg"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("logColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("warnColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("errorColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("level"));
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing, false);
            base.OnInspectorGUI();
        }
    }
}