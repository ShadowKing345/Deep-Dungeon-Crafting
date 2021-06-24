using Combat;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class WeaponClassEntityAiEditor : EditorWindow
    {
        private bool weaponClassIsNull = true;
        private SerializedObject _weaponClass;
        private WeaponClass.AbilityIndex _abilityIndex = WeaponClass.AbilityIndex.Abilities1;
        private string _selectedAbility = string.Empty;
        
        public static void Open(WeaponClass weaponClass)
        {
            WeaponClassEntityAiEditor window = GetWindow<WeaponClassEntityAiEditor>(weaponClass.name);
            window._weaponClass = new SerializedObject(weaponClass);
            window.weaponClassIsNull = false;
        }

        private void OnGUI()
        {
            if (weaponClassIsNull) return;
            
            DrawClassInformation();
            DrawActionsArea();
            _weaponClass.ApplyModifiedProperties();
        }

        private void DrawClassInformation()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.PropertyField(_weaponClass.FindProperty("icon"));
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    {
                        EditorGUILayout.PropertyField(_weaponClass.FindProperty("shortDescription"));
                        EditorGUILayout.PropertyField(_weaponClass.FindProperty("longDescription"));
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawActionsArea()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Abilities 1")) SelectAbilityArray(WeaponClass.AbilityIndex.Abilities1);
                    if (GUILayout.Button("Abilities 2")) SelectAbilityArray(WeaponClass.AbilityIndex.Abilities2);
                    if (GUILayout.Button("Abilities 3")) SelectAbilityArray(WeaponClass.AbilityIndex.Abilities3);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();
                {
                    DrawAbilityCollection();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        private void SelectAbilityArray(WeaponClass.AbilityIndex index)
        {
            _abilityIndex = index;
            _selectedAbility = string.Empty;
        }

        private void DrawAbilityCollection()
        {
            SerializedProperty currentProperty = _weaponClass.FindProperty(_abilityIndex switch
            {
                WeaponClass.AbilityIndex.Abilities1 => "abilities1",
                WeaponClass.AbilityIndex.Abilities2 => "abilities2",
                WeaponClass.AbilityIndex.Abilities3 => "abilities3",
                _ => "abilities1"
            });
            
            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.BeginVertical("box",GUILayout.Width(200));
                {
                    EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
                    {
                        foreach (SerializedProperty p in currentProperty)
                        {
                            if(GUILayout.Button(p.displayName)) _selectedAbility = p.propertyPath;
                        }
                    }
                    EditorGUILayout.EndVertical();
                    
                    EditorGUILayout.BeginHorizontal();
                    {
                        if(GUILayout.Button("Add Item")) currentProperty.arraySize++;
                        if(GUILayout.Button("Remove Item")) currentProperty.arraySize--;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.BeginVertical("box",GUILayout.ExpandWidth(true));
                {
                    if (!string.IsNullOrEmpty(_selectedAbility)) DrawAbilityDetails(_selectedAbility);
                    else EditorGUILayout.LabelField("Please select a Ability from the left.");
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAbilityDetails(string abilityPath)
        {
            SerializedProperty currentProperty = _weaponClass.FindProperty(abilityPath);
            
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative("icon"));
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative("name"));
                        EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative("description"));
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative("manaCost"));
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative("coolDown"));
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative("isProjectile"));
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative("projectilePreFab"));
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative("attackPoint"));
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative("range"));
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative("animationName"));
            }
            EditorGUILayout.EndVertical();
        }
    }

    [CustomEditor(typeof(WeaponClass))]
    public class WeaponClassInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor")) WeaponClassEntityAiEditor.Open((WeaponClass) target);
            base.OnInspectorGUI();
        }
    }
}