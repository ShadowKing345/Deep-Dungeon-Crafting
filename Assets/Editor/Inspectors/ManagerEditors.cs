using Entity;
using Entity.Player;
using Managers;
using UnityEditor;
using UnityEngine;

namespace Editor.Inspectors
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate Room"))
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                ((GameManager) target).SetupBoard();
            if(GUILayout.Button("Update Weapon")) FindObjectOfType<PlayerCombat>().UpdateCurrentWeaponClass();
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
            
            if (GUILayout.Button("Reset Lists")) manager.ResetBoard();
            if (GUILayout.Button("Init Variables")) manager.InitVariables();
            if (GUILayout.Button("Generate Board")) manager.CreateRoomLayout();
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            if (GUILayout.Button("Create Room Obj")) manager.CreateRoomObj();
            if (GUILayout.Button("Link Rooms")) manager.LinkRoomsTogether();
            if (GUILayout.Button("Create Rooms")) manager.GenerateRoomsTiles();
        }
    }
    
    [CustomEditor(typeof(EntityAi))]
    public class EntityAiEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            if(GUILayout.Button("Change State to current")) ((EntityAi) target).ChangeState(((EntityAi) target).GetCurrentlyThinking);
        }
    }
}