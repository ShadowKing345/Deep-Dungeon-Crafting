using UnityEditor;
using UnityEngine;

namespace Managers
{
    [CustomEditor(typeof(BoardManager))]
    public class BoardManagerEditor : UnityEditor.Editor
    {
        private bool foldOutButtons = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            BoardManager manager = target as BoardManager;

            if (manager == null) return;

            foldOutButtons = EditorGUILayout.Foldout(foldOutButtons, "Buttons");
            
            if(GUILayout.Button("Reset")) manager.ResetLists();
            if(GUILayout.Button("Initialize")) manager.InitializeVariables();
            if(GUILayout.Button("Generate Layout")) manager.GenerateLayout();
            if(GUILayout.Button("Create Rooms")) manager.CreateRooms();
            if(GUILayout.Button("Fill Roof")) manager.FillInRoof();
            if(GUILayout.Button("Connect Rooms")) manager.ConnectRooms();
            if(GUILayout.Button("Place Player")) manager.PlacePlayer();
        }
    }
}
