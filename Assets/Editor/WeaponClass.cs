using UnityEditor;
using UnityEngine;
using Weapons;

namespace Editor
{
    public class WeaponClassEditorWindow : ExtendedEditorWindow
    {
        private WeaponClass _weaponClass;
        
        public static void Open(WeaponClass weaponClass)
        {
            WeaponClassEditorWindow window = GetWindow<WeaponClassEditorWindow>(weaponClass.name);
            window._weaponClass = weaponClass;
        }

        private void OnGUI()
        {
            if (_weaponClass == null) return;
            SerializedObject ??= new SerializedObject(_weaponClass);

            DrawField("description");
            DrawField("icon");
            
            DrawField("action1");
            DrawField("action2");
            DrawField("action3");
        }
    }

    [CustomEditor(typeof(WeaponClass))]
    public class WeaponClassInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                WeaponClassEditorWindow.Open((WeaponClass) target);
            }
            base.OnInspectorGUI();
        }
    }
}