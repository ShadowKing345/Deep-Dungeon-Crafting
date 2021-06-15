using Managers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate Room"))
            {
                ((GameManager) target).SetupBoard();
            }
        }
    }
    
    [CustomEditor(typeof(BoardManager))]
    public class BoardManagerEditor : UnityEditor.Editor
    {
        private bool foldOutButtons = true;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            BoardManager manager = target as BoardManager;

            if(manager == null) return;

            foldOutButtons = EditorGUILayout.Foldout(foldOutButtons, "Buttons");
            
            if (!foldOutButtons) return;
            
            if (GUILayout.Button("Reset Lists")) manager.ResetLists();
            if (GUILayout.Button("Init Variables")) manager.InitVariables();
            if (GUILayout.Button("Generate Board")) manager.CreateRoomLayout();
            if (GUILayout.Button("Create Room Obj")) manager.CreateRoomObj();
            if (GUILayout.Button("Link Rooms")) manager.LinkRoomsTogether();
            if (GUILayout.Button("Create Rooms")) manager.GenerateRoomsTiles();
        }
    }
}