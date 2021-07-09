using System;
using System.Linq;
using Combat;
using UnityEditor;
using UnityEngine;

namespace Editor.EditorWindows
{
    public class WeaponClassEditorWindow : ScriptableObjectEditorWindow
    {
        private WeaponClass.AbilityIndex abilityIndex;
        private string selectedAbilityPath = string.Empty;
        
        protected static WeaponClassEditorWindow Open<T>(string windowName) where T : ScriptableObject
        {
            WeaponClassEditorWindow window = GetWindow<WeaponClassEditorWindow>(windowName);
            window.collection = AssetDatabase
                .FindAssets($"t:{typeof(T)}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray() as ScriptableObject[];
            window.OnCreate();
            return window;
        }

        protected static WeaponClassEditorWindow Open<T>(T selected, string windowName) where T : ScriptableObject
        {
            WeaponClassEditorWindow window = Open<T>(windowName);
            window.selected = new SerializedObject(selected);
            return window;
        }
        
        public static void Open() => Open<WeaponClass>("Weapon Class Editor Window");

        public static void Open(WeaponClass weaponClass) => Open(weaponClass, "Weapon Class Editor Window");

        protected internal override void OnCreate() => onCollectionSelectionChange += ResetVariables;

        private void OnGUI() => Draw(ContentDraw);

        private void ContentDraw()
        {
            EditorGUILayout.BeginHorizontal("box", GUILayout.Height(150));
            {
                EditorGUILayout.ObjectField(selected.FindProperty("icon"), typeof(Sprite), GUIContent.none, GUILayout.Width(150), GUILayout.ExpandHeight(true));
                EditorGUILayout.Space(10, false);
                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                {
                    EditorGUILayout.PropertyField(selected.FindProperty("shortDescription"));
                    EditorGUILayout.PropertyField(selected.FindProperty("longDescription"));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(10, false);
            
            DrawAbilities();
        }

        private void DrawAbilities()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    foreach (WeaponClass.AbilityIndex index in (WeaponClass.AbilityIndex[]) Enum.GetValues(typeof(WeaponClass.AbilityIndex)))
                        if (GUILayout.Button(ObjectNames.NicifyVariableName(index.ToString())))
                        {
                            abilityIndex = index;
                            selectedAbilityPath = string.Empty;
                        }
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(10,false);
                
                DrawAbility();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawAbility()
        {
            SerializedProperty selectedAbilities = selected.FindProperty(abilityIndex switch
            {
                WeaponClass.AbilityIndex.Abilities1 => "abilities1",
                WeaponClass.AbilityIndex.Abilities2 => "abilities2",
                WeaponClass.AbilityIndex.Abilities3 => "abilities3",
                _ => "ability1"
            });

            EditorGUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(true));
            {
                EditorGUILayout.BeginVertical("box", GUILayout.Width(120));
                {
                    EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
                    {
                        foreach (SerializedProperty p in selectedAbilities)
                            if (GUILayout.Button(p.displayName))
                                selectedAbilityPath = p.propertyPath;
                    }
                    EditorGUILayout.EndVertical();
                    
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Add")) selectedAbilities.arraySize++;
                        if (GUILayout.Button("Remove")) selectedAbilities.arraySize--;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
                {
                    DrawSelectedAbility();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSelectedAbility()
        {
            if (string.IsNullOrEmpty(selectedAbilityPath))
            {
                EditorGUILayout.LabelField("Please select ability from the left.");
                return;
            }

            SerializedProperty selectedAbility = selected.FindProperty(selectedAbilityPath);
            
            EditorGUILayout.BeginHorizontal("box", GUILayout.Height(120));
            {
                EditorGUILayout.ObjectField(selectedAbility.FindPropertyRelative("icon"), typeof(Sprite), GUIContent.none, GUILayout.Width(120), GUILayout.ExpandHeight(true));
                
                EditorGUILayout.Space(10, false);

                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                {
                    EditorGUILayout.PropertyField(selectedAbility.FindPropertyRelative("name"));
                    EditorGUILayout.PropertyField(selectedAbility.FindPropertyRelative("description"), GUILayout.ExpandHeight(true));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(selectedAbility.FindPropertyRelative("coolDown"));
                EditorGUILayout.PropertyField(selectedAbility.FindPropertyRelative("manaCost"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(selectedAbility.FindPropertyRelative("properties"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(selectedAbility.FindPropertyRelative("isProjectile"));
                EditorGUILayout.PropertyField(selectedAbility.FindPropertyRelative("projectilePreFab"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(selectedAbility.FindPropertyRelative("attackPoint"));
                EditorGUILayout.PropertyField(selectedAbility.FindPropertyRelative("range"));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(selectedAbility.FindPropertyRelative("animationName"));
            }
            EditorGUILayout.EndVertical();
        }

        private void ResetVariables()
        {
            selectedAbilityPath = string.Empty;
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