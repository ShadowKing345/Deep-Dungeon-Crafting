using System;
using Items;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ItemStack))]
    public class ItemStackDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label)  * 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            float oldX = position.x;
            
            // Draw Label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float fieldHeight = position.height / 4 - EditorGUIUtility.standardVerticalSpacing;
            
            Rect imageRect = new Rect(oldX, position.y + fieldHeight + EditorGUIUtility.standardVerticalSpacing, fieldHeight * 3, fieldHeight * 3);
            Rect itemRect = new Rect(position.x, position.y + fieldHeight + EditorGUIUtility.standardVerticalSpacing, position.width, fieldHeight);
            Rect amountRect = new Rect(position.x, position.y + fieldHeight * 2 + EditorGUIUtility.standardVerticalSpacing, position.width, fieldHeight);
            Rect maxStackSizeRect = new Rect(position.x, position.y + fieldHeight * 3 + EditorGUIUtility.standardVerticalSpacing, position.width, fieldHeight);
            
            if (property.FindPropertyRelative("item").objectReferenceValue is Item { } item)
                GUI.DrawTexture(imageRect, item.icon.texture);

            EditorGUI.PropertyField(itemRect, property.FindPropertyRelative("item"));
            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"));
            EditorGUI.PropertyField(maxStackSizeRect, property.FindPropertyRelative("maxStackSize"));

            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty();
        }
    }
}