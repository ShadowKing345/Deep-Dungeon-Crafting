using System;
using System.Linq;
using Combat;
using UnityEditor;
using UnityEngine;

namespace Editor.EditorWindows
{
    public class WeaponClassEditorWindow : EditorWindow
    {
        private WeaponClass[] collection;
        private SerializedObject selected;
        private string selectedProperty;

        public static WeaponClassEditorWindow Open()
        {
            WeaponClassEditorWindow window = GetWindow<WeaponClassEditorWindow>("Weapon Class Editor Window");
            window.collection = AssetDatabase
                .FindAssets($"t:{typeof(WeaponClass)}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<WeaponClass>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
            return window;
        }

        public static WeaponClassEditorWindow Open(WeaponClass @class)
        {
            WeaponClassEditorWindow window = Open();
            window.selected = new SerializedObject(@class);
            return window;
        }
        
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayoutOption[] verticalSlicesOptions = new[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)};
                
                EditorGUILayout.BeginVertical("box", verticalSlicesOptions.Concat(new []{GUILayout.MaxWidth(120)}).ToArray());
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.LabelField("Weapon Classes");
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical(verticalSlicesOptions.Concat(new []{GUILayout.MaxWidth(200)}).ToArray());
                    {
                        foreach (WeaponClass @class in collection)
                            if (GUILayout.Button(@class.name))
                                selected = new SerializedObject(@class);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical("box", verticalSlicesOptions);
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.LabelField("Variables");
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical(verticalSlicesOptions);
                    {
                        if(selected != null)
                            DrawClassProperties();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical("box", verticalSlicesOptions);
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.LabelField("Abilities");
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical();
                    {
                        if(!string.IsNullOrEmpty(selectedProperty))
                            DrawProperty();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawClassProperties()
        {
            if(GUILayout.Button(selected.FindProperty("shortDescription").displayName)) selectedProperty = "shortDescription";
            if(GUILayout.Button(selected.FindProperty("longDescription").displayName)) selectedProperty = "longDescription";
            if(GUILayout.Button(selected.FindProperty("icon").displayName)) selectedProperty = "icon";

            if(GUILayout.Button(selected.FindProperty("abilities1").displayName)) selectedProperty = "abilities1";
            if(GUILayout.Button(selected.FindProperty("abilities2").displayName)) selectedProperty = "abilities2";
            if(GUILayout.Button(selected.FindProperty("abilities3").displayName)) selectedProperty = "abilities3";
        }

        private void DrawProperty()
        {
            EditorGUILayout.PropertyField(selected.FindProperty(selectedProperty));
        }
    }

    [CustomEditor(typeof(WeaponClass))]
    public class WeaponClassInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor")) WeaponClassEditorWindow.Open((WeaponClass) target);
            base.OnInspectorGUI();
        }
    }
}