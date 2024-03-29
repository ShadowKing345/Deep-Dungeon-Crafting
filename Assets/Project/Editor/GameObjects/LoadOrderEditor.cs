using Project.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace Project.Editor.GameObjects
{
    [CustomEditor(typeof(LoadOrder))]
    
    public class LoadOrderEditor : UnityEditor.Editor
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("PreInit")) LoadOrder.preInit?.Invoke();
            if (GUILayout.Button("Init")) LoadOrder.init?.Invoke();
            if (GUILayout.Button("PostInit")) LoadOrder.postInit?.Invoke();
        }
    }
}