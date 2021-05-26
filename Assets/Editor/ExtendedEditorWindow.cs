using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ExtendedEditorWindow: EditorWindow
    {
        
        protected SerializedObject SerializedObject;
        protected SerializedProperty CurrentProperty;

        private string _selectedPropertyPath;
        protected SerializedProperty SelectedProperty;

        protected void DrawProperties(SerializedProperty prop, bool drawChildren)
        {
            string lastPropPath = string.Empty;
            foreach (SerializedProperty p in prop)
            {
                if (p.isArray & p.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (p.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if(!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) continue;

                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }

        protected void DrawSideBar(SerializedProperty prop)
        {
            foreach (SerializedProperty p in prop)
            {
                if (GUILayout.Button(p.displayName))
                    _selectedPropertyPath = p.propertyPath;
            }

            if (!string.IsNullOrEmpty(_selectedPropertyPath))
            {
                SelectedProperty = SerializedObject.FindProperty(_selectedPropertyPath);
            }
        }

        protected void DrawField(string propName, bool relative = true)
        {
            if (relative && CurrentProperty != null)
            {
                EditorGUILayout.PropertyField(CurrentProperty.FindPropertyRelative(propName), true);
            }else if (SerializedObject != null)
            {
                EditorGUILayout.PropertyField(SerializedObject.FindProperty(propName), true);
            }
        }
        
        protected void Apply()
        {
            SerializedObject.ApplyModifiedProperties();
        }
    }
}