using System;
using Collections;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class WeaponClassesEditorWindow : ExtendedEditorWindow
    {
        private int _action = 1;
        
        private string _selectedActionPropertyPath;

        public static void Open(WeaponClasses weaponClasses)
        {
            WeaponClassesEditorWindow window = GetWindow<WeaponClassesEditorWindow>("Weapon Class Editor");
            window.SerializedObject = new SerializedObject(weaponClasses);
        }

        private void OnGUI()
        {
            if (SerializedObject == null) return;
            CurrentProperty = SerializedObject.FindProperty("gameData");

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
                {
                    DrawSideBar(CurrentProperty);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
                {
                    if (SelectedProperty != null)
                    {
                        DrawSelectedSidePanel();
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Select an Item from the List.");
                    }

                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            Apply();
        }

        private void DrawSelectedSidePanel()
        {
            CurrentProperty = SelectedProperty;

            EditorGUILayout.BeginHorizontal("box");
            {

                EditorGUILayout.BeginHorizontal("box");
                {
                    DrawField("classIcon", true);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
                {
                    DrawField("name", true);
                    DrawField("description", true);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("Action 1", EditorStyles.toolbarButton))
                {
                    _selectedActionPropertyPath = string.Empty;
                    _action = 1;
                }

                if (GUILayout.Button("Action 2", EditorStyles.toolbarButton))
                {
                    _selectedActionPropertyPath = string.Empty;
                    _action = 2;
                }

                if (GUILayout.Button("Action 3", EditorStyles.toolbarButton))
                {
                    _selectedActionPropertyPath = string.Empty;
                    _action = 3;
                }
            }
            EditorGUILayout.EndHorizontal();

            switch (_action)
            {
                case 1:
                    DrawActionElements(CurrentProperty.FindPropertyRelative("action1"));
                    break;
                case 2:
                    DrawActionElements(CurrentProperty.FindPropertyRelative("action2"));
                    break;
                case 3:
                    DrawActionElements(CurrentProperty.FindPropertyRelative("action3"));
                    break;
            }
        }

        private void DrawActionElements(SerializedProperty prop)
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                    
                EditorGUILayout.BeginVertical("box",GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
                foreach (SerializedProperty p in prop)
                {
                    if (GUILayout.Button(p.displayName))
                        _selectedActionPropertyPath = p.propertyPath;
                }

                EditorGUILayout.EndVertical();
                    
                EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

                if (!string.IsNullOrEmpty(_selectedActionPropertyPath))
                {
                    EditorGUILayout.BeginHorizontal("box");
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            DrawField(_selectedActionPropertyPath + ".actionIcon", false);
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                        {
                            DrawField(_selectedActionPropertyPath + ".name", false);
                            DrawField(_selectedActionPropertyPath + ".description", false);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginVertical("box");
                    {
                        DrawField(_selectedActionPropertyPath + ".potency", false);
                        DrawField(_selectedActionPropertyPath + ".range", false);
                        DrawField(_selectedActionPropertyPath + ".isProjectile", false);
                        DrawField(_selectedActionPropertyPath + ".projectilePreFab", false);
                        DrawField(_selectedActionPropertyPath + ".coolDown", false);
                        DrawField(_selectedActionPropertyPath + ".attackType", false);
                        DrawField(_selectedActionPropertyPath + ".elementType", false);
                        DrawField(_selectedActionPropertyPath + ".attackPoint", false);
                    }
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.LabelField("Select an Action from the List.");
                }
                
                EditorGUILayout.EndVertical();
                    
                EditorGUILayout.EndHorizontal();

            }
            EditorGUILayout.EndVertical();
        }
    }
}