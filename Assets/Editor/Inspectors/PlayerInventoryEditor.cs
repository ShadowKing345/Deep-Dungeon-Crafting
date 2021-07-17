using Ui.Inventories.InventoryControllers;
using UnityEditor;
using UnityEngine;

namespace Editor.Inspectors
{
    [CustomEditor(typeof(PlayerInventoryController))]
    public class PlayerInventoryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("SetUp Slots")) ((PlayerInventoryController) target).SetUpSlots();
            if(GUILayout.Button("Update Slots")) ((PlayerInventoryController) target).UpdateSlots();
            if(GUILayout.Button("Reset Slots")) ((PlayerInventoryController) target).ResetSlots();
            base.OnInspectorGUI();
        }
    }
}