using System;
using UnityEditor;
using UnityEngine;
using Weapons;

namespace Editor
{
    public class WeaponClassEditorWindow : ExtendedEditorWindow
    {
        private WeaponClass _weaponClass;
        private Vector2 _bodyScroll;
        private int _actionIndex = 1;

        private void OnEnable() => AssetHandler.Subscribe(typeof(WeaponClass), o => Open((WeaponClass) o));
        private void OnDisable() => AssetHandler.Unsubscribe(typeof(WeaponClass));

        public static void Open(WeaponClass weaponClass)
        {
            WeaponClassEditorWindow window = GetWindow<WeaponClassEditorWindow>(weaponClass.name);
            window._weaponClass = weaponClass;
        }

        private void OnGUI()
        {
            if (_weaponClass == null) return;
            SerializedObject ??= new SerializedObject(_weaponClass);

            EditorGUILayout.BeginVertical();
            {
                _bodyScroll = EditorGUILayout.BeginScrollView(_bodyScroll);
                {
                    DrawField("description");
                    DrawField("icon");

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("Action 1")) _actionIndex = 1;
                            if (GUILayout.Button("Action 2")) _actionIndex = 2;
                            if (GUILayout.Button("Action 3")) _actionIndex = 3;
                        }
                        EditorGUILayout.EndHorizontal();

                        DrawAction();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            Apply();
        }

        private void DrawAction()
        {
            EditorGUILayout.BeginVertical();
            {
                DrawProperties(SerializedObject.FindProperty($"action{_actionIndex}"), true);
            }
            EditorGUILayout.EndVertical();

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