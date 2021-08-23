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
                ((GameManager) target).StartLevel();
            if (GUILayout.Button("Update Weapon")) FindObjectOfType<PlayerCombat>().UpdateCurrentWeaponClass();
            if (GUILayout.Button("FinishRun")) ((GameManager) target).FinishRun();
        }
    }

    [CustomEditor(typeof(EntityAiBase))]
    public class EntityAiEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            if(GUILayout.Button("Change State to current")) ((EntityAiBase) target).ChangeState(((EntityAiBase) target).GetCurrentlyThinking);
        }
    }
}