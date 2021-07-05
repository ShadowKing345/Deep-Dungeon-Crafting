using Combat;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(AbilityProperty))]
    public class AbilityPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2 + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect startingRect = position;
            
            position = EditorGUI.PrefixLabel(position, label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            float baseHeight = base.GetPropertyHeight(property, label);

            const float offset = 18;
            position.x += offset;
            position.width -= offset;

            Rect amountRect = new Rect(position.position, new Vector2(position.width, baseHeight));

            position.y += baseHeight + EditorGUIUtility.standardVerticalSpacing;
            
            Rect isElementalRect = new Rect(startingRect.x, position.y, position.width, baseHeight);
            Rect dropdownRect = new Rect(position.x, position.y, position.width, baseHeight);

            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"));
            EditorGUI.PropertyField(isElementalRect, property.FindPropertyRelative("isElemental"));
            EditorGUI.PropertyField(dropdownRect,
                property.FindPropertyRelative(property.FindPropertyRelative("isElemental").boolValue
                    ? "element"
                    : "attackType"));

            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty();
        }
    }
}