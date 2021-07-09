using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.EditorWindows
{
    public abstract class ScriptableObjectEditorWindow : EditorWindow
    {
        protected Action onCollectionSelectionChange;

        internal ScriptableObject[] collection;
        protected internal SerializedObject selected;
        private bool IsSelectedNull => selected == null;

        private Vector2 navigationScrollPos;
        private Vector2 contentScrollPos;

        protected internal abstract void OnCreate();

        protected void Draw(Action contentDraw)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.Width(120));
                {
                    navigationScrollPos = EditorGUILayout.BeginScrollView(navigationScrollPos);
                    {
                        foreach (var tab in collection)
                            if (GUILayout.Button(tab.name))
                            {
                                onCollectionSelectionChange?.Invoke();
                                selected = new SerializedObject(tab);
                            }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                {
                    contentScrollPos = EditorGUILayout.BeginScrollView(contentScrollPos);
                    {
                        if (IsSelectedNull)
                            EditorGUILayout.LabelField("Please select an item from the left.");
                        else
                        {
                            contentDraw();
                            ApplyModifiedChanges();
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ApplyModifiedChanges()
        {
            if (!IsSelectedNull) selected.ApplyModifiedProperties();
        }
        
    }
}