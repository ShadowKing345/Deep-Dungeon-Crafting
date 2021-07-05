using System;
using Items;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ItemStack))]
    public class ItemStackDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => (base.GetPropertyHeight(property, label) + EditorGUIUtility.standardVerticalSpacing) * 3 ;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            Rect startingRect = position;
            position.height = base.GetPropertyHeight(property, label);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            Rect imageRect = new Rect(new Vector2(startingRect.x, position.y),
                Vector2.one * (position.height * 2 + EditorGUIUtility.standardVerticalSpacing));

            if (property.FindPropertyRelative("item").objectReferenceValue is Item {Icon: { } sprite} item)
            {
                if (item.Icon != null)
                {
                    Texture2D texture = new Texture2D((int) sprite.rect.width, (int) sprite.rect.height);
                    var pixels = sprite.texture.GetPixels((int) sprite.textureRect.x, (int) sprite.textureRect.y,
                        (int) sprite.textureRect.width, (int) sprite.textureRect.height);

                    texture.SetPixels(pixels);
                    texture.Apply();

                    GUI.DrawTexture(imageRect, texture);
                }
            }

            EditorGUI.PropertyField(position, property.FindPropertyRelative("item"));
            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("amount"));

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}