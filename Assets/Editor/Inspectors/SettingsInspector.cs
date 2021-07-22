using Settings;
using Ui.Menu;
using UnityEditor;
using UnityEngine;

namespace Editor.Inspectors
{
    [CustomEditor(typeof(SettingsController))]
    public class SettingsInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Save Settings")) ((SettingsController) target).SaveSettings();
        }
    }
}