using System;
using UnityEditor;
using UnityEngine;
using Weapons;

namespace Editor
{
    public class WeaponClassEditorWindow : ExtendedEditorWindow
    {
        private Texture2D _missingTexture;
        private string _selectedActionsPropertyPath = string.Empty;
        private string _selectedActionPropertyPath = string.Empty;

        private Vector2 _actionScrollPos;

        private void SetWeaponClass(WeaponClass weaponClass)
        {
            if (weaponClass == null)
            {
                Debug.LogWarning("Hmm...");
                return;
            }

            SerializedObject = new SerializedObject(weaponClass);
        }

        private void OnEnable() =>
            _missingTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/Missing_Texture.png");

        public static void Open(WeaponClass weaponClass)
        {
            WeaponClassEditorWindow window = GetWindow<WeaponClassEditorWindow>(weaponClass.name);
            window.SetWeaponClass(weaponClass);
        }

        private void OnGUI()
        {
            if (SerializedObject == null) return;

            SerializedObject.Update();

            DrawClass(SerializedObject);

            Apply();
        }

        public void DrawClass(SerializedObject obj)
        {
            EditorGUILayout.BeginHorizontal("box", GUILayout.MaxHeight(150));
            {
                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(100));
                {
                    Sprite sprite = (Sprite) obj.FindProperty("icon").objectReferenceValue;
                    GUILayout.Label(sprite != null ? sprite.texture : _missingTexture, GUILayout.ExpandHeight(true));
                    EditorGUILayout.PropertyField(obj.FindProperty("icon"), GUIContent.none);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField("Description:");
                    EditorGUILayout.PropertyField(obj.FindProperty("description"), GUIContent.none,
                        GUILayout.ExpandHeight(true));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Action 1", EditorStyles.toolbarButton))
                    {
                        _selectedActionsPropertyPath = obj.FindProperty("action1").propertyPath;
                        _selectedActionPropertyPath = string.Empty;
                    }

                    if (GUILayout.Button("Action 2", EditorStyles.toolbarButton))
                    {
                        _selectedActionsPropertyPath = obj.FindProperty("action2").propertyPath;
                        _selectedActionPropertyPath = string.Empty;
                    }

                    if (GUILayout.Button("Action 3", EditorStyles.toolbarButton))
                    {
                        _selectedActionsPropertyPath = obj.FindProperty("action3").propertyPath;
                        _selectedActionPropertyPath = string.Empty;
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();
                {
                    _actionScrollPos = EditorGUILayout.BeginScrollView(_actionScrollPos);
                    DrawActions(obj);
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawActions(SerializedObject obj)
        {
            if (string.IsNullOrEmpty(_selectedActionsPropertyPath)) return;
            CurrentProperty = obj.FindProperty(_selectedActionsPropertyPath);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical("box", GUILayout.MinWidth(150), GUILayout.MaxWidth(150),
                    GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    foreach (SerializedProperty p in CurrentProperty)
                    {
                        if (GUILayout.Button(p.displayName, EditorStyles.toolbarButton))
                            _selectedActionPropertyPath = p.propertyPath;
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    if (!string.IsNullOrEmpty(_selectedActionPropertyPath))
                    {
                        DrawAction(obj);
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Please select an item from the left.");
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAction(SerializedObject obj)
        {
            if (string.IsNullOrEmpty(_selectedActionPropertyPath)) return;
            CurrentProperty = obj.FindProperty(_selectedActionPropertyPath);

            EditorGUILayout.BeginHorizontal("box", GUILayout.MaxHeight(150));
            {
                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(100));
                {
                    Sprite sprite = (Sprite) CurrentProperty.FindPropertyRelative("icon").objectReferenceValue;
                    GUILayout.Label(sprite != null ? sprite.texture : _missingTexture, GUILayout.ExpandHeight(true));
                    EditorGUILayout.PropertyField(obj.FindProperty("icon"), GUIContent.none);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField("Name:");
                    EditorGUILayout.PropertyField(CurrentProperty.FindPropertyRelative("name"), GUIContent.none);
                    EditorGUILayout.LabelField("Description:");
                    EditorGUILayout.PropertyField(CurrentProperty.FindPropertyRelative("description"), GUIContent.none,
                        GUILayout.ExpandHeight(true));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            {
                DrawField("animationName");
                DrawField("attackPoint");
                DrawField("coolDown");
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical("box");
            {
                DrawField("isProjectile");

                EditorGUILayout.BeginVertical("box");
                if (CurrentProperty.FindPropertyRelative("isProjectile").boolValue)
                {
                    DrawField("projectilePreFab");
                }
                else
                {
                    DrawField("potency");
                    DrawField("range");
                    DrawField("attackType");
                    DrawField("elementType");
                }

                EditorGUILayout.EndVertical();
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

    public class WeaponClassesEditor : ExtendedEditorWindow
    {
        private ObjectHolder _data;
        private WeaponClassEditorWindow _editorWindow;
        private string _selectedClassPropertyPath = string.Empty;

        private void OnEnable()
        {
            _data = CreateInstance<ObjectHolder>();
            _editorWindow = CreateInstance<WeaponClassEditorWindow>();
        }

        [MenuItem("Tools/Weapon Classes Editors")]
        public static void Init()
        {
            WeaponClassesEditor window = GetWindow<WeaponClassesEditor>("Weapon Class Editor");
            window._data.weaponClasses = Resources.LoadAll<WeaponClass>("");
            window.SerializedObject = new SerializedObject(window._data);
        }

        private void OnGUI()
        {
            if (SerializedObject == null) return;
            CurrentProperty = SerializedObject.FindProperty("weaponClasses");
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    foreach (SerializedProperty p in CurrentProperty)
                        if (GUILayout.Button(p.displayName))
                            _selectedClassPropertyPath = p.propertyPath;
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    if (!string.IsNullOrEmpty(_selectedClassPropertyPath))
                        _editorWindow.DrawClass(new SerializedObject(SerializedObject.FindProperty(_selectedClassPropertyPath).objectReferenceValue));
                    else
                        EditorGUILayout.LabelField("Empty");
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private class ObjectHolder : ScriptableObject
        {
            public WeaponClass[] weaponClasses;
        }
    }
}