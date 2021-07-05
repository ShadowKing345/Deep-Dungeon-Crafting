using System;
using System.Linq;
using Items;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    public class ItemEditorWindow : EditorWindow
    {
        private Item[] _collection;
        private SerializedObject _selectedItem;
        
        public static void Open(Item item)
        {
            ItemEditorWindow window = GetWindow<ItemEditorWindow>("Item Editor Window");
            Open();
            window._selectedItem = new SerializedObject(item);
        }

        public static void Open()
        {
            ItemEditorWindow window = GetWindow<ItemEditorWindow>("Item Editor Window");
            window._collection = AssetDatabase.FindAssets($"t:{typeof(Item)}").Select(guid =>
                AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid))).ToArray();
        }

        private void OnGUI()
        {
            if (_collection == null) return;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.Width(120));
                {
                    foreach (Item item in _collection)
                    {
                        if (GUILayout.Button(item.name)) _selectedItem = new SerializedObject(item);
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                {
                    if (_selectedItem != null)
                        DrawItem();
                    else
                        EditorGUILayout.LabelField("Please select an Item from the left.");
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawItem()
        {
            EditorGUILayout.BeginHorizontal("box", GUILayout.Height(150 + EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight));
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(150));
                {
                    GUILayout.Space(150);

                    Rect rect = new Rect(GUILayoutUtility.GetLastRect().position + Vector2.right * 30, Vector2.one * 150);
                    
                    Sprite sprite = (Sprite) _selectedItem.FindProperty("icon").objectReferenceValue;

                    if (sprite == null)
                        GUI.DrawTexture(new Rect(rect.position, Vector2.one * 150), EditorGUIUtility.whiteTexture);
                    else
                    {
                        Texture2D texture = new Texture2D((int) sprite.rect.width, (int) sprite.rect.height);
                        var pixels = sprite.texture.GetPixels((int) sprite.textureRect.x, (int) sprite.textureRect.y,
                            (int) sprite.textureRect.width, (int) sprite.textureRect.height);

                        texture.SetPixels(pixels);
                        texture.Apply();

                        GUI.DrawTexture(rect, texture);
                    }

                    GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

                    EditorGUILayout.ObjectField(_selectedItem.FindProperty("icon"), typeof(Sprite), GUIContent.none);
                }
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.Space(5, false);
                EditorGUILayout.BeginVertical( GUILayout.ExpandWidth(true));
                {
                    EditorGUILayout.PropertyField(_selectedItem.FindProperty("maxStackSize"));
                    EditorGUILayout.Space(1, false);
                    EditorGUILayout.PrefixLabel("Description:");
                    EditorGUILayout.Space(2, false);
                    EditorGUILayout.PropertyField(_selectedItem.FindProperty("description"), GUIContent.none, GUILayout.ExpandHeight(true));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            switch (_selectedItem.targetObject)
            {
                case WeaponItem _:
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.PrefixLabel("Weapon Item:");

                    EditorGUILayout.PropertyField(_selectedItem.FindProperty("weaponClass"));
                    EditorGUILayout.PropertyField(_selectedItem.FindProperty("properties"));
                    
                    EditorGUILayout.EndVertical();
                    break;
                case ArmorItem _:
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.PrefixLabel("Armor Item:");

                    EditorGUILayout.PropertyField(_selectedItem.FindProperty("type"));
                    EditorGUILayout.PropertyField(_selectedItem.FindProperty("properties"));
                    
                    EditorGUILayout.EndVertical();
                    break;
                default:
                    break;
            }

            _selectedItem.ApplyModifiedProperties();
        }
    }
}